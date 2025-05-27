# README.md
# Typester 🧠⌨️

A fun and simple console-based typing test built with .NET.

## 🚀 Quick Start

### 1. Download

Go to the [Releases](https://github.com/mahdirad82/typester/releases) page and download the archive for your platform:

- **macOS:** `typester-osx-arm64.zip`
- **Linux:** `typester-linux-x64.zip`

### 2. Extract

You should see at least:

- `typester` (the CLI executable)
- `words.db` (the SQLite database, required)
- `libe_sqlite3.dylib` (macOS only; required)
- `libe_sqlite3.so` (Linux only; required)
- _Possibly other platform libraries_

### 3. Make Executable

If not already executable, set the permission:

```sh
chmod +x typester
```

## 📝 Usage

```bash
typester -n 30
```

## 🛠️ Build Manually

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```


## 📄 License
MIT
