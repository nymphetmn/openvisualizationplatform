
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ -debug -define:DEBUG
ASSEMBLY = ../build/OpenVP.GtkGui.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../build/OpenVP.dll \
	../build/OpenVP.Core.dll \
	../build/OpenVP.Scripting.dll
BUILD_DIR = ../build

OPENVP_DLL_SOURCE=../build/OpenVP.dll
OPENVP_DLL_MDB_SOURCE=../build/OpenVP.dll.mdb
OPENVP_DLL_MDB=$(BUILD_DIR)/OpenVP.dll.mdb
OPENVP_CORE_DLL_SOURCE=../build/OpenVP.Core.dll
CDH_AFFE_DLL_SOURCE=../contrib/Cdh.Affe.dll
OPENVP_GTKGUI_EXE_MDB_SOURCE=../build/OpenVP.GtkGui.exe.mdb
OPENVP_GTKGUI_EXE_MDB=$(BUILD_DIR)/OpenVP.GtkGui.exe.mdb
OPENVP_CORE_DLL_MDB_SOURCE=../build/OpenVP.Core.dll.mdb
OPENVP_CORE_DLL_MDB=$(BUILD_DIR)/OpenVP.Core.dll.mdb
OPENVP_SCRIPTING_DLL_SOURCE=../build/OpenVP.Scripting.dll
OPENVP_SCRIPTING_DLL_MDB_SOURCE=../build/OpenVP.Scripting.dll.mdb
OPENVP_SCRIPTING_DLL_MDB=$(BUILD_DIR)/OpenVP.Scripting.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = ../build/OpenVP.GtkGui.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../build/OpenVP.dll \
	../build/OpenVP.Core.dll \
	../build/OpenVP.Scripting.dll
BUILD_DIR = ../build

OPENVP_DLL_SOURCE=../build/OpenVP.dll
OPENVP_DLL_MDB=
OPENVP_CORE_DLL_SOURCE=../build/OpenVP.Core.dll
CDH_AFFE_DLL_SOURCE=../contrib/Cdh.Affe.dll
OPENVP_GTKGUI_EXE_MDB=
OPENVP_CORE_DLL_MDB=
OPENVP_SCRIPTING_DLL_SOURCE=../build/OpenVP.Scripting.dll
OPENVP_SCRIPTING_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=.resources.dll

PROGRAMFILES = \
	$(OPENVP_DLL) \
	$(OPENVP_DLL_MDB) \
	$(OPENVP_CORE_DLL) \
	$(CDH_AFFE_DLL) \
	$(OPENVP_GTKGUI_EXE_MDB) \
	$(OPENVP_CORE_DLL_MDB) \
	$(OPENVP_SCRIPTING_DLL) \
	$(OPENVP_SCRIPTING_DLL_MDB)  

BINARIES = \
	$(OPENVP_GTKGUI)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES =  \
	AssemblyInfo.cs \
	BrowsableEditor.cs \
	BrowsableItem.cs \
	EffectSelectorDialog.cs \
	gtk-gui/generated.cs \
	gtk-gui/OpenVP.GtkGui.BrowsableEditor.cs \
	gtk-gui/OpenVP.GtkGui.EffectSelectorDialog.cs \
	gtk-gui/OpenVP.GtkGui.LinearPresetEditor.cs \
	gtk-gui/OpenVP.GtkGui.MainWindow.cs \
	gtk-gui/OpenVP.GtkGui.MemberEditor.cs \
	KeyEntry.cs \
	LinearPresetEditor.cs \
	Main.cs \
	MainWindow.cs \
	MemberEditor.cs \
	MemberEditors/BooleanEditor.cs \
	MemberEditors/ColorEditor.cs \
	MemberEditors/EnumEditor.cs \
	MemberEditors/NumericEditor.cs \
	MemberEditors/ScriptEditor.cs \
	MemberEditors/StringEditor.cs \
	MemberEditors/UnknownEditor.cs \
	Util.cs 

DATA_FILES = 

RESOURCES =  \
	gtk-gui/gui.stetic \
	gtk-gui/objects.xml 

EXTRAS = \
	openvp.gtkgui.in 

REFERENCES =  \
	../build/OpenVP.Core.dll \
	../build/OpenVP.dll \
	../build/OpenVP.Scripting.dll \
	Mono.Posix \
	-pkg:glade-sharp-2.0 \
	-pkg:glib-sharp-2.0 \
	-pkg:gtk-sharp-2.0 \
	-pkg:Tao.Sdl \
	System \
	System.Runtime.Serialization.Formatters.Soap

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

OPENVP_GTKGUI = $(BUILD_DIR)/openvp.gtkgui
OPENVP_DLL = $(BUILD_DIR)/OpenVP.dll
OPENVP_CORE_DLL = $(BUILD_DIR)/OpenVP.Core.dll
CDH_AFFE_DLL = $(BUILD_DIR)/Cdh.Affe.dll
OPENVP_SCRIPTING_DLL = $(BUILD_DIR)/OpenVP.Scripting.dll

$(eval $(call emit-deploy-wrapper,OPENVP_GTKGUI,openvp.gtkgui,x))
$(eval $(call emit-deploy-target,CDH_AFFE_DLL))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
