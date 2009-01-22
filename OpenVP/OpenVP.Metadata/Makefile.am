
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/OpenVP.Metadata.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

OPENVP_METADATA_DLL_MDB_SOURCE=bin/Debug/OpenVP.Metadata.dll.mdb
OPENVP_METADATA_DLL_MDB=$(BUILD_DIR)/OpenVP.Metadata.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/OpenVP.Metadata.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

OPENVP_METADATA_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=.resources.dll

PROGRAMFILES = \
	$(OPENVP_METADATA_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(OPENVP_METADATA_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES =  \
	AssemblyInfo.cs \
	Channel.cs \
	ChannelWindow.cs \
	Event.cs \
	IChannel.cs \
	LooseSourceAttribute.cs \
	MetadataManager.cs \
	MetadataObject.cs \
	OpenVP.Metadata.Formatters/JSONFormatter.cs \
	OpenVP.Metadata.Loose/LooseArray.cs \
	OpenVP.Metadata.Loose/LooseDictionary.cs \
	OpenVP.Metadata.Loose/LooseNumber.cs \
	OpenVP.Metadata.Loose/LooseObject.cs \
	OpenVP.Metadata.Loose/LooseString.cs \
	StaticChannel.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	openvp.metadata.pc.in 

REFERENCES =  \
	System \
	System.Core

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

OPENVP_METADATA_PC = $(BUILD_DIR)/openvp.metadata.pc

$(eval $(call emit-deploy-wrapper,OPENVP_METADATA_PC,openvp.metadata.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
