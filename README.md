# 🎨 ThemeForge — Редактор тем рабочего стола

> Красивая Windows-утилита для настройки тем, иконок, обоев и фоновой музыки

![Скриншот](Assets/preview.png)

---

## ✨ Возможности

| Функция | Описание |
|--------|----------|
| 🎨 **Темы** | 8 готовых тёмных тем (Dark Red, Ocean Blue, Cyberpunk...) |
| 📦 **Иконки** | Выбор цвета иконок (тёмно-красный, синий, зелёный и др.) |
| 🖼 **Обои** | Автоматическая установка обоев под тему, свои картинки |
| 🎵 **Музыка** | Тихая фоновая музыка, свои треки, плеер в боковом меню |
| 🖱 **Курсор** | Выбор стиля курсора |
| ✏ **Шрифты** | Предпросмотр системных шрифтов |

---

## 🚀 Быстрый старт

### 1. Клонировать репозиторий
```bash
git clone https://github.com/ВАШ_НИК/ThemeForge.git
cd ThemeForge
```

### 2. Запустить (два способа)

**Способ А — PowerShell (рекомендуется):**
```
Правой кнопкой на install_and_run.ps1 → "Запустить с помощью PowerShell"
```

**Способ Б — bat файл:**
```
Двойной клик на run.bat
```

**Способ В — вручную:**
```bash
dotnet run --project ThemeForge.csproj
```

---

## 📋 Требования

- **Windows 10 / 11**
- **[.NET 6+ SDK](https://dotnet.microsoft.com/download)** (скрипт предложит установить автоматически)

---

## 📁 Структура папок

```
ThemeForge/
├── 📄 ThemeForge.csproj     — файл проекта
├── 📄 App.xaml              — стили и ресурсы
├── 📄 MainWindow.xaml       — интерфейс окна
├── 📄 MainWindow.xaml.cs    — логика приложения
├── 🚀 run.bat               — запуск (Windows)
├── 🚀 install_and_run.ps1   — установка + запуск
│
├── 🖼 Wallpapers/           — ← СЮДА кладёшь обои (png/jpg)
│   ├── dark_red.png
│   ├── ocean_blue.png
│   └── ...
│
├── 🎵 Music/                — ← СЮДА кладёшь музыку (mp3/wav)
│   ├── ambient_dark.mp3
│   └── ...
│
└── 🗂 Assets/               — иконки, превью
    └── icon.ico
```

---

## 🖼 Добавление своих обоев

1. Положи любое изображение (`.png`, `.jpg`, `.bmp`) в папку `Wallpapers/`
2. Переименуй под одну из тем:

| Тема | Файл обоев |
|------|-----------|
| Dark Red | `dark_red.png` |
| Ocean Blue | `ocean_blue.png` |
| Forest Green | `forest.png` |
| Royal Purple | `purple.png` |
| Midnight | `midnight.png` |
| Neon Cyber | `cyberpunk.png` |
| Sunset | `sunset.png` |
| Arctic | `arctic.png` |

Или просто используй кнопку **"📂 Выбрать файл..."** на вкладке "Обои".

---

## 🎵 Добавление музыки

1. Положи любой `.mp3` или `.wav` файл в папку `Music/`
2. Перезапусти приложение — треки появятся в плеере автоматически

Или нажми **"📂 Добавить свой трек"** прямо в приложении.

### Рекомендуемые источники бесплатной музыки:
- [Pixabay Music](https://pixabay.com/music/) — бесплатно, без авторских прав
- [Free Music Archive](https://freemusicarchive.org/) — ambient, lo-fi
- [YouTube Audio Library](https://studio.youtube.com/channel/UCi9/music)

---

## 🛠 Разработка и изменение кода

Открыть в Visual Studio:
```bash
# Открыть папку проекта в VS
start ThemeForge.csproj
```

Или в VS Code:
```bash
code .
```

Основные файлы для изменения:
- `MainWindow.xaml` — дизайн интерфейса (XML/XAML)
- `MainWindow.xaml.cs` — логика (C#)
- `App.xaml` — цвета и стили

---

## 📦 Как выложить на GitHub

```bash
# 1. Создай репозиторий на https://github.com/new

# 2. В терминале в папке проекта:
git init
git add .
git commit -m "Initial commit: ThemeForge v1.0"
git branch -M main
git remote add origin https://github.com/ВАШ_НИК/ThemeForge.git
git push -u origin main
```

---

## ❓ Частые вопросы

**Q: Приложение не запускается**  
A: Убедись что установлен .NET 6+ SDK: `dotnet --version`

**Q: Обои не устанавливаются**  
A: Это нормально если файл ещё не добавлен в папку Wallpapers/

**Q: Музыка не играет**  
A: Добавь mp3 файлы в папку Music/

**Q: Нужны права администратора?**  
A: Для установки обоев — нет. Для смены курсора — да.

---

## 📄 Лицензия

MIT — делай что хочешь 🎉

---

*Сделано с ❤️ на C# / WPF*
