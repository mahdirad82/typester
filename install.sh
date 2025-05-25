# install.sh
#!/bin/bash
set -e

DEST="$HOME/.local/bin"
echo "📦 Installing Typester to $DEST..."
mkdir -p "$DEST"

OS=$(uname -s)
case "$OS" in
  Linux*)   PLATFORM="linux-x64";;
  Darwin*)  PLATFORM="osx-x64";;
  *)        echo "❌ Unsupported OS: $OS"; exit 1;;
esac

URL="https://github.com/YOUR_USERNAME/typester/releases/latest/download/typester-${PLATFORM}"

curl -L -o "$DEST/typester" "$URL"
chmod +x "$DEST/typester"

echo "✅ Typester installed!"
echo "👉 Run it using: typester -n 30"
echo "👉 If not in PATH, add this to ~/.zshrc or ~/.bashrc:"
echo 'export PATH="$HOME/.local/bin:$PATH"'

