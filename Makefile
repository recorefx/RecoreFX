DOCFX_INSTALL_DIR := bin/docfx

.PHONY: all
all: build

.PHONY: build
build:
	dotnet build

.PHONY: install-docfx
install-docfx:
	curl -L -O https://github.com/dotnet/docfx/releases/download/v2.56.2/docfx.zip
	unzip docfx.zip -d $(DOCFX_INSTALL_DIR)
	rm docfx.zip
	chmod +x docfx/docfx.exe

.PHONY: docs
docs:
	$(DOCFX_INSTALL_DIR)/docfx docs/docfx/docfx.json
