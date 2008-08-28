// DynamicMovement.cs
//
//  Copyright (C) 2007-2008 Chris Howie
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;
using System.Runtime.Serialization;
using Tao.OpenGl;
using Cdh.Affe;
using OpenVP.Scripting;
using OpenVP.Metadata;

namespace OpenVP.Core {
	[Serializable, DisplayName("Dynamic movement"), Category("Transform"),
	 Description("Applies a movement function to the buffer."),
	 Author("Chris Howie")]
	public class DynamicMovement : MovementBase {
		private AffeScript mInitScript = new AffeScript();
		
		[Browsable(true), DisplayName("Init"),
		 Category("Scripts"),
		 Description("This script is executed once, before any others.")]
		public AffeScript InitScript {
			get {
				return this.mInitScript;
			}
		}
		
		[NonSerialized]
		private bool mNeedInit = true;
		
		private AffeScript mFrameScript = new AffeScript();
		
		[Browsable(true), DisplayName("Frame"),
		 Category("Scripts"), Follows("InitScript"),
		 Description("This script is executed once each frame, before the vertex script.")]
		public AffeScript FrameScript {
			get {
				return this.mFrameScript;
			}
		}
		
		private AffeScript mBeatScript = new AffeScript();
		
		[Browsable(true), DisplayName("On beat"),
		 Category("Scripts"), Follows("FrameScript"),
		 Description("This script is executed after the frame script when a beat is detected.")]
		public AffeScript BeatScript {
			get {
				return this.mBeatScript;
			}
		}
		
		private AffeScript mVertexScript = new AffeScript();
		
		[Browsable(true), DisplayName("Vertex"),
		 Category("Scripts"), Follows("BeatScript"),
		 Description("This script is executed for each vertex in the grid.")]
		public AffeScript VertexScript {
			get {
				return this.mVertexScript;
			}
		}
		
		[NonSerialized]
		private ScriptHost mScriptHost;
		
		[Browsable(true), DisplayName("X"), Category("Grid resolution"),
		 Range(2, 512),
		 Description("The number of verticies along the X axis.")]
		public new int XResolution {
			get { return base.XResolution; }
			set { base.XResolution = value; }
		}
		
		[Browsable(true), DisplayName("Y"), Category("Grid resolution"),
		 Range(2, 512),
		 Description("The number of verticies along the Y axis.")]
		public new int YResolution {
            get { return base.YResolution; }
            set { base.YResolution = value; }
		}
		
		[Browsable(true), DisplayName("Wrap"), Category("Miscellaneous"),
		 Description("Whether to wrap when the scripts compute points that are off the screen.")]
		public new bool Wrap {
			get { return base.Wrap; }
			set { base.Wrap = value; }
		}
		
		private bool mRectangular = false;
		
		[Browsable(true), DisplayName("Rectangular"), Category("Miscellaneous"),
		 Description("Whether to use rectangular coordinates instead of polar.")]
		public bool Rectangular {
			get {
				return this.mRectangular;
			}
			set {
				this.mRectangular = value;
                this.MakeStaticDirty();
			}
		}
		
		[Browsable(true), DisplayName("Static motion"), Category("Miscellaneous"),
		 Description("Whether the motion can change over time (off) or is static (on).")]
		public new bool Static {
			get { return base.Static; }
			set { base.Static = value; }
		}
		
		public DynamicMovement() {
			this.InitializeScriptObjects();
		}
		
		protected override void OnDeserialization(object sender) {
            base.OnDeserialization(sender);
            
			this.InitializeScriptObjects();
		}
		
		private void InitializeScriptObjects() {
			AffeCompiler compiler = new AffeCompiler(typeof(ScriptHost));
			
			ScriptingEnvironment.InstallBase(compiler);
			ScriptingEnvironment.InstallMath(compiler);
			
			this.mScriptHost = new ScriptHost();
			
			this.mInitScript.Compiler = compiler;
			this.mInitScript.TargetObject = this.mScriptHost;
			
			this.mFrameScript.Compiler = compiler;
			this.mFrameScript.TargetObject = this.mScriptHost;
			
			this.mBeatScript.Compiler = compiler;
			this.mBeatScript.TargetObject = this.mScriptHost;
			
			this.mVertexScript.Compiler = compiler;
			this.mVertexScript.TargetObject = this.mScriptHost;
			
			this.mNeedInit = true;
            this.MakeStaticDirty();
			
			this.mInitScript.MadeDirty += this.OnInitMadeDirty;
			this.mFrameScript.MadeDirty += this.OnOtherMadeDirty;
			this.mVertexScript.MadeDirty += this.OnOtherMadeDirty;
		}
		
		private void OnInitMadeDirty(object o, EventArgs e) {
			this.mNeedInit = true;
            this.MakeStaticDirty();
		}
		
		private void OnOtherMadeDirty(object o, EventArgs e) {
			this.MakeStaticDirty();
		}
		
		private static bool RunScript(UserScript script, string type) {
			try {
				ScriptCall call = script.Call;
				if (call == null)
					return false;
				
				call();
			} catch (Exception e) {
				Console.WriteLine("Exception executing the {0} script:", type);
				Console.WriteLine(e.ToString());
				return false;
			}
			
			return true;
		}

        protected override void OnRenderFrame() {
            if (this.mNeedInit) {
                this.mNeedInit = false;
                RunScript(this.InitScript, "initialization");
            }

            RunScript(this.FrameScript, "frame");
        }

        protected override void OnBeat() {
            RunScript(this.BeatScript, "beat");
        }

        protected override void PlotVertex(MovementData data) {
            this.mScriptHost.X = data.X;
            this.mScriptHost.XI = data.X;
            this.mScriptHost.Y = data.Y;
            this.mScriptHost.YI = data.Y;
            this.mScriptHost.D = data.Distance;
            this.mScriptHost.R = data.Rotation;

            data.Method = this.Rectangular ? MovementMethod.Rectangular : MovementMethod.Polar;

            if (RunScript(this.VertexScript, "vertex")) {
                if (this.Rectangular) {
                    data.X = this.mScriptHost.X;
                    data.Y = this.mScriptHost.Y;
                } else {
                    data.Distance = this.mScriptHost.D;
                    data.Rotation = this.mScriptHost.R;
                }

                data.Alpha = this.mScriptHost.Alpha;
            }
        }
		
		[Serializable]
		private class ScriptHost : ISerializable {
			[AffeBound]
			public ScriptState State = new ScriptState();
			
			[AffeBound("x")]
			public float X = 0;
			
			[AffeBound("y")]
			public float Y = 0;
			
			[AffeBound("xi")]
			public float XI = 0;
			
			[AffeBound("yi")]
			public float YI = 0;
			
			[AffeBound("alpha")]
			public float Alpha = 1;
			
			[AffeBound("d")]
			public float D = 0;
			
			[AffeBound("r")]
			public float R = 0;
			
			void ISerializable.GetObjectData(SerializationInfo info,
			                                 StreamingContext context) {
			}
			
			protected ScriptHost(SerializationInfo info,
			                     StreamingContext context) {
			}
			
			public ScriptHost() {
			}
		}
	}
}
