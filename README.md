# README.md
# Typester 🧠⌨️

A fun and simple console-based typing test built with .NET.

## 🚀 Install

```bash
curl -sSL https://raw.githubusercontent.com/mahdirad82/typester/main/install.sh | bash
```

Make sure `$HOME/.local/bin` is in your PATH.

## 📝 Usage

```bash
typester -n 30
```

## 🛠️ Build Manually

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```

## 💥 Contribute
Pull requests welcome! Add new modes, stats, or features.

## 📄 License
MIT
