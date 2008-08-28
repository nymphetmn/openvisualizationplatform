// MovementBase.cs
//
//  Copyright (C) 2008 Chris Howie
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

using OpenVP;
using OpenVP.Metadata;

using Tao.OpenGl;

namespace OpenVP.Core {
    [Serializable, Browsable(false)]
    public abstract class MovementBase : Effect {
        private int mXResolution = 16;
        
        protected virtual int XResolution {
            get {
                return this.mXResolution;
            }
            set {
                if (value < 2)
                    throw new ArgumentOutOfRangeException("value < 2");
                
                this.mXResolution = value;
                this.CreatePointDataArray();
            }
        }
        
        private int mYResolution = 16;
        
        protected virtual int YResolution {
            get {
                return this.mYResolution;
            }
            set {
                if (value < 2)
                    throw new ArgumentOutOfRangeException("value < 2");
                
                this.mYResolution = value;
                this.CreatePointDataArray();
            }
        }
        
        private bool mWrap = true;
        
        protected virtual bool Wrap {
            get {
                return this.mWrap;
            }
            set {
                this.mWrap = value;
            }
        }
        
        private bool mStatic = false;
        
        protected virtual bool Static {
            get {
                return this.mStatic;
            }
            set {
                this.mStatic = value;
            }
        }
        
        [NonSerialized]
        private bool mStaticDirty = true;
        
        public MovementBase() {
            this.CreatePointDataArray();
        }
        
        protected override void OnDeserialization(object sender) {
            base.OnDeserialization(sender);
            
            this.CreatePointDataArray();
        }
        
        private void CreatePointDataArray() {
            this.MakeStaticDirty();

            this.UpdateCaches();
        }
        
        protected void MakeStaticDirty() {
            this.mStaticDirty = true;
        }
        
        protected virtual void OnRenderFrame() {
        }
        
        protected virtual void OnBeat() {
        }
        
        protected abstract void PlotVertex(MovementData data);
        
        public override void NextFrame(IController controller) {
            if (!this.mStatic || this.mStaticDirty) {
                this.OnRenderFrame();
                
                if (controller.BeatDetector.IsBeat)
                    this.OnBeat();
            }
        }

        private struct ValueCache {
            public float X;
            public float Y;
            public float XP;
            public float YP;
            public float Distance;
            public float Rotation;
        }
        
        [NonSerialized]
        private ValueCache[] mCache = null;

        [NonSerialized]
        private float[] mVertexCache = null;

        [NonSerialized]
        private int mVertexVBO = -1;

        [NonSerialized]
        private float[] mTexCoordCache = null;

        [NonSerialized]
        private uint[] mIndexCache = null;

        [NonSerialized]
        private int mIndexVBOSize = 0;

        [NonSerialized]
        private int mIndexVBO = -1;
        
        private void UpdateCaches() {
            this.mCache = new ValueCache[this.XResolution * this.YResolution];
            
            this.mVertexCache = new float[this.XResolution * this.YResolution * 2];
            this.mTexCoordCache = new float[this.XResolution * this.YResolution * 2];

            int i = 0;
            int vi = 0;
            
            for (int yi = 0; yi < this.YResolution; yi++) {
                for (int xi = 0; xi < this.XResolution; xi++) {
                    ValueCache cache;

                    cache.X = (float) xi / (this.XResolution - 1);
                    cache.Y = (float) yi / (this.YResolution - 1);
                    
                    cache.XP = cache.X * 2 - 1;
                    cache.YP = cache.Y * 2 - 1;

                    float xp = cache.XP;
                    float yp = cache.YP;

                    cache.Distance = (float) Math.Sqrt((xp * xp) + (yp * yp));
                    cache.Rotation = (float) Math.Atan2(yp, xp);

                    this.mCache[i++] = cache;
                    
                    this.mVertexCache[vi++] = xp;
                    this.mVertexCache[vi++] = yp;
                }
            }

            this.mIndexCache = new uint[(this.XResolution - 1) *
                                        (this.YResolution - 1) * 4];

            i = 0;
            uint ii = 0;
            uint ii2 = checked((uint) this.YResolution);
            
            for (int yi = 0; yi < this.YResolution - 1; yi++) {
                for (int xi = 0; xi < this.XResolution - 1; xi++) {
                    this.mIndexCache[i++] = ii;
                    this.mIndexCache[i++] = ++ii;
                    this.mIndexCache[i++] = ii2 + 1;
                    this.mIndexCache[i++] = ii2++;
                }

                ++ii;
                ++ii2;
            }

            /* Console.WriteLine("--- Index cache for {0} x {1} ---", this.XResolution, this.YResolution);

            try {
            for (i = 0; i < this.mIndexCache.Length; i += 4) {
                Console.WriteLine("{0},{1},{2},{3} ({4},{5}) ({6},{7}) ({8},{9}) ({10},{11})",
                                  this.mIndexCache[i],
                                  this.mIndexCache[i + 1],
                                  this.mIndexCache[i + 2],
                                  this.mIndexCache[i + 3],

                                  this.mVertexCache[this.mIndexCache[i] * 2],
                                  this.mVertexCache[this.mIndexCache[i] * 2 + 1],

                                  this.mVertexCache[this.mIndexCache[i + 1] * 2],
                                  this.mVertexCache[this.mIndexCache[i + 1] * 2 + 1],

                                  this.mVertexCache[this.mIndexCache[i + 2] * 2],
                                  this.mVertexCache[this.mIndexCache[i + 2] * 2 + 1],

                                  this.mVertexCache[this.mIndexCache[i + 3] * 2],
                                  this.mVertexCache[this.mIndexCache[i + 3] * 2 + 1]
                                  );
            }
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("FAIL");
            } */
        }

        [NonSerialized]
        private bool? haveVBO = null;
        
        public override void RenderFrame(IController controller) {
            if (this.haveVBO == null)
                this.haveVBO = Gl.IsExtensionSupported("GL_ARB_vertex_buffer_object");
            
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();
            
            Gl.glPushAttrib(Gl.GL_ENABLE_BIT);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);
            
            this.mTexture.SetTextureSize(controller.Width,
                                         controller.Height);
            
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.mTexture.TextureId);
            
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S,
                               this.Wrap ? Gl.GL_REPEAT : Gl.GL_CLAMP);
            
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T,
                               this.Wrap ? Gl.GL_REPEAT : Gl.GL_CLAMP);
            
            Gl.glCopyTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, 0, 0,
                                controller.Width, controller.Height, 0);
            
            MovementData data = new MovementData();
            
            if (!this.mStatic || this.mStaticDirty) {
                int cachei = 0;
                int texi = 0;
                
                for (int yi = 0; yi < this.YResolution; yi++) {
                    for (int xi = 0; xi < this.XResolution; xi++) {
                        ValueCache cache = this.mCache[cachei++];
                        
                        data.X = cache.X;
                        data.Y = cache.Y;
                        
                        data.Distance = cache.Distance;
                        data.Rotation = cache.Rotation;
                        
                        this.PlotVertex(data);

                        float xo, yo;
                        
                        if (data.Method == MovementMethod.Rectangular) {
                            xo = data.X;
                            yo = data.Y;
                        } else {
                            xo = (data.Distance * (float) Math.Cos(data.Rotation) + 1) / 2;
                            yo = (data.Distance * (float) Math.Sin(data.Rotation) + 1) / 2;
                        }
                        
                        //pd.Alpha = data.Alpha;

                        this.mTexCoordCache[texi++] = xo;
                        this.mTexCoordCache[texi++] = yo;
                    }
                }
                
                this.mStaticDirty = false;
            }
            
            Gl.glColor4f(1, 1, 1, 1);

            if (this.haveVBO ?? false) {
                if (this.mVertexCache != null) {
                    if (this.mVertexVBO >= 0) {
                        Gl.glDeleteBuffersARB(1, ref this.mVertexVBO);
                    }
                    
                    Gl.glGenBuffersARB(1, out this.mVertexVBO);
                    Gl.glBindBufferARB(Gl.GL_ARRAY_BUFFER_ARB, this.mVertexVBO);
                    Gl.glBufferDataARB(Gl.GL_ARRAY_BUFFER_ARB,
                                       new IntPtr(this.mVertexCache.Length * 4),
                                       this.mVertexCache,
                                       Gl.GL_STATIC_DRAW_ARB);

                    this.mVertexCache = null;
                    
                    Gl.glBindBufferARB(Gl.GL_ARRAY_BUFFER_ARB, 0);
                }

                if (this.mIndexCache != null) {
                    if (this.mIndexVBO >= 0) {
                        Gl.glDeleteBuffersARB(1, ref this.mIndexVBO);
                    }

                    Gl.glGenBuffersARB(1, out this.mIndexVBO);
                    Gl.glBindBufferARB(Gl.GL_ELEMENT_ARRAY_BUFFER_ARB, this.mIndexVBO);
                    Gl.glBufferDataARB(Gl.GL_ELEMENT_ARRAY_BUFFER_ARB,
                                       new IntPtr(this.mIndexCache.Length * 4),
                                       this.mIndexCache,
                                       Gl.GL_STATIC_DRAW_ARB);

                    this.mIndexVBOSize = this.mIndexCache.Length;
                    
                    this.mIndexCache = null;
                }
            }
            
            Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
            
            Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, this.mTexCoordCache);

            if (this.haveVBO ?? false) {
                Gl.glBindBufferARB(Gl.GL_ARRAY_BUFFER_ARB, this.mVertexVBO);
                Gl.glBindBufferARB(Gl.GL_ELEMENT_ARRAY_BUFFER_ARB, this.mIndexVBO);
                
                Gl.glVertexPointer(2, Gl.GL_FLOAT, 0, IntPtr.Zero);

                Gl.glDrawElements(Gl.GL_QUADS,
                                  this.mIndexVBOSize,
                                  Gl.GL_UNSIGNED_INT,
                                  IntPtr.Zero);
                
                Gl.glBindBufferARB(Gl.GL_ARRAY_BUFFER_ARB, 0);
                Gl.glBindBufferARB(Gl.GL_ELEMENT_ARRAY_BUFFER_ARB, 0);
            } else {
                Gl.glVertexPointer(2, Gl.GL_FLOAT, 0, this.mVertexCache);
                
                Gl.glDrawElements(Gl.GL_QUADS,
                                  this.mIndexCache.Length,
                                  Gl.GL_UNSIGNED_INT,
                                  this.mIndexCache);
            }

            Gl.glDisableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glDisableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
            
            Gl.glPopAttrib();
            
            Gl.glPopMatrix();
        }
        
        public override void Dispose() {
            if (mHasTextureRef) {
                mHasTextureRef = false;
                
                mTextureHandle.RemoveReference();
            }

            if (this.mVertexVBO >= 0) {
                Gl.glDeleteBuffersARB(1, ref this.mVertexVBO);
                this.mVertexVBO = -1;
            }

            if (this.mIndexVBO >= 0) {
                Gl.glDeleteBuffersARB(1, ref this.mIndexVBO);
                this.mIndexVBO = -1;
                this.mIndexVBOSize = 0;
            }
        }
        
        ~MovementBase() {
            this.Dispose();
        }
        
        [NonSerialized]
        private bool mHasTextureRef = false;
        
        private TextureHandle mTexture {
            get {
                if (!this.mHasTextureRef) {
                    this.mHasTextureRef = true;
                    
                    mTextureHandle.AddReference();
                }
                
                return mTextureHandle;
            }
        }
        
        private static SharedTextureHandle mTextureHandle = new SharedTextureHandle();
        
        public enum MovementMethod : byte {
            Rectangular,
            Polar
        }
        
        public class MovementData {
            public MovementData() {
            }
            
            private MovementMethod mMethod = MovementMethod.Rectangular;
            
            public MovementMethod Method {
                get { return this.mMethod; }
                set { this.mMethod = value; }
            }
            
            private float mX;
            
            public float X {
                get { return this.mX; }
                set { this.mX = value; }
            }
            
            private float mY;
            
            public float Y {
                get { return this.mY; }
                set { this.mY = value; }
            }
            
            private float mAlpha = 1;
            
            public float Alpha {
                get { return this.mAlpha; }
                set { this.mAlpha = value; }
            }
            
            private float mRotation;
            
            public float Rotation {
                get { return this.mRotation; }
                set { this.mRotation = value; }
            }
            
            private float mDistance;
            
            public float Distance {
                get { return this.mDistance; }
                set { this.mDistance = value; }
            }
        }
    }
}
