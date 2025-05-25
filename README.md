# README.md
# Typester 🧠⌨️

A fun and simple console-based typing test built with .NET.

## 🚀 Quick Start

### 1. Download

Go to the [Releases](https://github.com/mahdirad82/typester/releases) page and download the archive for your platform:

- **macOS:** `typester-osx-arm64.zip`
- **Linux:** `typester-linux-x64.zip`

### 2. Extract

Example for macOS/Linux:

```sh
# For ZIP (macOS)
unzip typester-macos-x64.zip

# For ZIP (Linux)
unzip typester-linux-x64.zip   
```

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

### 4. (Optional) Install Globally

If you want to use `typester` from anywhere in your terminal, move it (and supporting files) to a directory in your `PATH`, such as `/usr/local/bin` (macOS/Linux):

```sh
# Move the binary
sudo mv typester /usr/local/bin/

# (macOS) Also move the SQLite library
sudo mv libe_sqlite3.dylib /usr/local/bin/
# (Linux) Also move the SQLite library
sudo mv libe_sqlite3.so /usr/local/bin/

# Copy the database file
sudo mv words.db /usr/local/bin/
```

> ⚠️ The `words.db` database must be in the same directory as the `typester` executable.
> For updates, remember to replace all necessary files.

### 5. Run

Now you can invoke `typester` from anywhere:


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
