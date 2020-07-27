DOCFX_INSTALL_DIR := bin/docfx

.PHONY: all
all: build install-docfx docs

.PHONY: build
build:
	dotnet build

$(DOCFX_INSTALL_DIR):
	mkdir -p $@

.PHONY: install-docfx
install-docfx: $(DOCFX_INSTALL_DIR)/docfx.exe

$(DOCFX_INSTALL_DIR)/docfx.exe: | $(DOCFX_INSTALL_DIR)
	curl -L -O https://github.com/dotnet/docfx/releases/download/v2.56.2/docfx.zip
	unzip docfx.zip -d $(DOCFX_INSTALL_DIR)
	rm docfx.zip
	chmod +x $(DOCFX_INSTALL_DIR)/docfx.exe

.PHONY: docs
docs:
	$(DOCFX_INSTALL_DIR)/docfx docs/docfx/docfx.json
