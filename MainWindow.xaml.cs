using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace ThemeForge
{
    // ─────────────────────────────────────────────────────
    //  Модель данных темы
    // ─────────────────────────────────────────────────────
    public class AppTheme
    {
        public string Name        { get; set; }
        public string Emoji       { get; set; }
        public string Description { get; set; }
        public Color  PrimaryColor  { get; set; }
        public Color  AccentColor   { get; set; }
        public string WallpaperFile { get; set; }  // имя файла в папке Wallpapers
        public string MusicTrack    { get; set; }  // имя файла в папке Music
    }

    // ─────────────────────────────────────────────────────
    //  Модель данных цвета иконок
    // ─────────────────────────────────────────────────────
    public class IconColorOption
    {
        public string Name  { get; set; }
        public Color  Color { get; set; }
    }

    // ─────────────────────────────────────────────────────
    //  Главное окно
    // ─────────────────────────────────────────────────────
    public partial class MainWindow : Window
    {
        // Плеер
        private System.Windows.Media.MediaPlayer _player = new System.Windows.Media.MediaPlayer();
        private bool   _isPlaying   = false;
        private int    _trackIndex  = 0;

        // Треки (пути)
        private List<string> _tracks = new List<string>();

        // Текущая выбранная тема
        private AppTheme _currentTheme;

        // Список всех тем
        private List<AppTheme> _themes = new List<AppTheme>
        {
            new AppTheme
            {
                Name = "Dark Red",
                Emoji = "🔴",
                Description = "Тёмно-красный • Кровь дракона",
                PrimaryColor = Color.FromRgb(0x8B, 0x00, 0x00),
                AccentColor  = Color.FromRgb(0xE9, 0x45, 0x60),
                WallpaperFile = "dark_red.png",
                MusicTrack    = "ambient_dark.mp3"
            },
            new AppTheme
            {
                Name = "Ocean Blue",
                Emoji = "🔵",
                Description = "Тёмно-синий • Глубины океана",
                PrimaryColor = Color.FromRgb(0x00, 0x1A, 0x4E),
                AccentColor  = Color.FromRgb(0x00, 0x96, 0xFF),
                WallpaperFile = "ocean_blue.png",
                MusicTrack    = "ambient_ocean.mp3"
            },
            new AppTheme
            {
                Name = "Forest Green",
                Emoji = "🟢",
                Description = "Тёмно-зелёный • Лесная тишь",
                PrimaryColor = Color.FromRgb(0x0D, 0x2B, 0x00),
                AccentColor  = Color.FromRgb(0x39, 0xD3, 0x53),
                WallpaperFile = "forest.png",
                MusicTrack    = "ambient_forest.mp3"
            },
            new AppTheme
            {
                Name = "Royal Purple",
                Emoji = "🟣",
                Description = "Тёмно-фиолетовый • Королевский",
                PrimaryColor = Color.FromRgb(0x1A, 0x00, 0x4E),
                AccentColor  = Color.FromRgb(0xA0, 0x55, 0xFF),
                WallpaperFile = "purple.png",
                MusicTrack    = "ambient_night.mp3"
            },
            new AppTheme
            {
                Name = "Midnight",
                Emoji = "⚫",
                Description = "Полночь • Чистая темнота",
                PrimaryColor = Color.FromRgb(0x05, 0x05, 0x05),
                AccentColor  = Color.FromRgb(0x60, 0x60, 0x60),
                WallpaperFile = "midnight.png",
                MusicTrack    = "ambient_space.mp3"
            },
            new AppTheme
            {
                Name = "Neon Cyber",
                Emoji = "🌆",
                Description = "Неоновый • Киберпанк",
                PrimaryColor = Color.FromRgb(0x00, 0x08, 0x20),
                AccentColor  = Color.FromRgb(0xFF, 0x00, 0xCC),
                WallpaperFile = "cyberpunk.png",
                MusicTrack    = "ambient_cyber.mp3"
            },
            new AppTheme
            {
                Name = "Sunset",
                Emoji = "🌅",
                Description = "Закат • Тёплые тона",
                PrimaryColor = Color.FromRgb(0x2D, 0x10, 0x00),
                AccentColor  = Color.FromRgb(0xFF, 0x7B, 0x00),
                WallpaperFile = "sunset.png",
                MusicTrack    = "ambient_warm.mp3"
            },
            new AppTheme
            {
                Name = "Arctic",
                Emoji = "❄",
                Description = "Арктика • Ледяная чистота",
                PrimaryColor = Color.FromRgb(0x00, 0x1A, 0x2E),
                AccentColor  = Color.FromRgb(0x00, 0xE5, 0xFF),
                WallpaperFile = "arctic.png",
                MusicTrack    = "ambient_ice.mp3"
            }
        };

        // Цвета иконок
        private List<IconColorOption> _iconColors = new List<IconColorOption>
        {
            new IconColorOption { Name = "Тёмно-красный",     Color = Color.FromRgb(0xC0, 0x00, 0x00) },
            new IconColorOption { Name = "Тёмно-синий",       Color = Color.FromRgb(0x00, 0x33, 0x99) },
            new IconColorOption { Name = "Тёмно-зелёный",     Color = Color.FromRgb(0x00, 0x66, 0x00) },
            new IconColorOption { Name = "Фиолетовый",        Color = Color.FromRgb(0x66, 0x00, 0xCC) },
            new IconColorOption { Name = "Оранжевый",         Color = Color.FromRgb(0xE6, 0x55, 0x00) },
            new IconColorOption { Name = "Розовый неон",      Color = Color.FromRgb(0xFF, 0x00, 0x99) },
            new IconColorOption { Name = "Золотой",           Color = Color.FromRgb(0xCC, 0xAA, 0x00) },
            new IconColorOption { Name = "Серебряный",        Color = Color.FromRgb(0x80, 0x80, 0x90) },
        };

        // ─────────────────────────────────────────────────────
        //  Инициализация
        // ─────────────────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetStatus("Добро пожаловать в ThemeForge!");

            BuildThemeCards();
            BuildIconCards();
            BuildWallpaperCards();
            BuildCursorCards();
            BuildFontCards();
            LoadMusicList();

            // Выбрать первую тему по умолчанию
            ApplyTheme(_themes[0]);

            // Запустить анимацию
            var fadeIn = (Storyboard)FindResource("FadeIn");
            fadeIn?.Begin(this);
        }

        // ─────────────────────────────────────────────────────
        //  Построение карточек тем
        // ─────────────────────────────────────────────────────
        private void BuildThemeCards()
        {
            ThemePanel.Children.Clear();
            foreach (var theme in _themes)
            {
                var card = CreateThemeCard(theme);
                ThemePanel.Children.Add(card);
            }
        }

        private Border CreateThemeCard(AppTheme theme)
        {
            // Градиентный фон карточки
            var brush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint   = new Point(1, 1)
            };
            brush.GradientStops.Add(new GradientStop(theme.PrimaryColor, 0));
            brush.GradientStops.Add(new GradientStop(theme.AccentColor,  1));

            var card = new Border
            {
                Width         = 185,
                Height        = 130,
                Margin        = new Thickness(0, 0, 12, 12),
                CornerRadius  = new CornerRadius(10),
                Background    = brush,
                Cursor        = Cursors.Hand,
                BorderThickness = new Thickness(2),
                BorderBrush   = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255))
            };

            var inner = new StackPanel
            {
                Margin              = new Thickness(15),
                VerticalAlignment   = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var emojiTxt = new TextBlock
            {
                Text     = theme.Emoji,
                FontSize = 28,
                Margin   = new Thickness(0, 0, 0, 5)
            };

            var nameTxt = new TextBlock
            {
                Text       = theme.Name,
                FontSize   = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            };

            var descTxt = new TextBlock
            {
                Text       = theme.Description,
                FontSize   = 10,
                Foreground = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255)),
                Margin     = new Thickness(0, 2, 0, 0)
            };

            inner.Children.Add(emojiTxt);
            inner.Children.Add(nameTxt);
            inner.Children.Add(descTxt);
            card.Child = inner;

            // Hover-эффект
            card.MouseEnter += (s, e) =>
            {
                card.BorderBrush = new SolidColorBrush(Colors.White);
                card.Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = theme.AccentColor, BlurRadius = 20, Opacity = 0.8, ShadowDepth = 0
                };
            };
            card.MouseLeave += (s, e) =>
            {
                card.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                card.Effect = null;
            };

            // Клик — применить тему
            card.MouseLeftButtonUp += (s, e) => ApplyTheme(theme);

            return card;
        }

        // ─────────────────────────────────────────────────────
        //  Построение карточек иконок
        // ─────────────────────────────────────────────────────
        private void BuildIconCards()
        {
            IconPanel.Children.Clear();
            foreach (var option in _iconColors)
            {
                var card = CreateIconColorCard(option);
                IconPanel.Children.Add(card);
            }
        }

        private Border CreateIconColorCard(IconColorOption option)
        {
            var card = new Border
            {
                Width           = 140,
                Height          = 80,
                Margin          = new Thickness(0, 0, 12, 12),
                CornerRadius    = new CornerRadius(8),
                Background      = new SolidColorBrush(Color.FromRgb(0x16, 0x21, 0x3E)),
                Cursor          = Cursors.Hand,
                BorderThickness = new Thickness(2),
                BorderBrush     = new SolidColorBrush(Color.FromRgb(0x2D, 0x35, 0x61))
            };

            var inner = new StackPanel
            {
                Margin              = new Thickness(12),
                VerticalAlignment   = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var colorDot = new Border
            {
                Width        = 20,
                Height       = 20,
                CornerRadius = new CornerRadius(10),
                Background   = new SolidColorBrush(option.Color),
                Margin       = new Thickness(0, 0, 0, 6)
            };

            var nameTxt = new TextBlock
            {
                Text       = option.Name,
                FontSize   = 12,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap
            };

            inner.Children.Add(colorDot);
            inner.Children.Add(nameTxt);
            card.Child = inner;

            card.MouseEnter += (s, e) =>
                card.BorderBrush = new SolidColorBrush(option.Color);
            card.MouseLeave += (s, e) =>
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(0x2D, 0x35, 0x61));

            card.MouseLeftButtonUp += (s, e) => ApplyIconColor(option);

            return card;
        }

        // ─────────────────────────────────────────────────────
        //  Построение карточек обоев
        // ─────────────────────────────────────────────────────
        private void BuildWallpaperCards()
        {
            WallpaperPanel.Children.Clear();
            string wallDir = GetAssetPath("Wallpapers");

            // Показываем существующие файлы ИЛИ заглушки
            var wallpapers = new List<(string name, string file)>
            {
                ("Dark Red",    "dark_red.png"),
                ("Ocean",       "ocean_blue.png"),
                ("Forest",      "forest.png"),
                ("Purple",      "purple.png"),
                ("Midnight",    "midnight.png"),
                ("Cyberpunk",   "cyberpunk.png"),
                ("Sunset",      "sunset.png"),
                ("Arctic",      "arctic.png"),
            };

            foreach (var (name, file) in wallpapers)
            {
                string fullPath = Path.Combine(wallDir, file);
                var card = CreateWallpaperCard(name, fullPath);
                WallpaperPanel.Children.Add(card);
            }
        }

        private Border CreateWallpaperCard(string name, string path)
        {
            Border card = new Border
            {
                Width           = 160,
                Height          = 100,
                Margin          = new Thickness(0, 0, 12, 12),
                CornerRadius    = new CornerRadius(8),
                Background      = new SolidColorBrush(Color.FromRgb(0x16, 0x21, 0x3E)),
                Cursor          = Cursors.Hand,
                BorderThickness = new Thickness(2),
                BorderBrush     = new SolidColorBrush(Color.FromRgb(0x2D, 0x35, 0x61)),
                ClipToBounds    = true
            };

            // Если файл существует — показываем превью
            if (File.Exists(path))
            {
                try
                {
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(path, UriKind.Absolute);
                    img.DecodePixelWidth = 160;
                    img.EndInit();

                    card.Background = new ImageBrush
                    {
                        ImageSource = img,
                        Stretch     = Stretch.UniformToFill
                    };
                }
                catch { /* если изображение битое — оставляем цвет */ }
            }

            // Подпись
            var overlay = new Border
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Background        = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)),
                Padding           = new Thickness(8, 4, 8, 4)
            };
            overlay.Child = new TextBlock
            {
                Text       = name,
                Foreground = Brushes.White,
                FontSize   = 11
            };

            card.Child = overlay;

            card.MouseEnter += (s, e) =>
                card.BorderBrush = new SolidColorBrush(Colors.White);
            card.MouseLeave += (s, e) =>
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(0x2D, 0x35, 0x61));

            card.MouseLeftButtonUp += (s, e) =>
            {
                if (File.Exists(path))
                    SetWallpaper(path);
                else
                    SetStatus($"Файл обоев не найден: {path}");
            };

            return card;
        }

        // ─────────────────────────────────────────────────────
        //  Карточки курсоров
        // ─────────────────────────────────────────────────────
        private void BuildCursorCards()
        {
            CursorPanel.Children.Clear();
            var options = new[] { "Стандартный", "Стрелка", "Рука", "Перекрестие", "Точка" };
            foreach (var opt in options)
            {
                var card = new Border
                {
                    Width = 130, Height = 80, Margin = new Thickness(0, 0, 12, 12),
                    CornerRadius = new CornerRadius(8), Cursor = Cursors.Hand,
                    Background = new SolidColorBrush(Color.FromRgb(0x16, 0x21, 0x3E)),
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0x2D, 0x35, 0x61))
                };
                card.Child = new TextBlock
                {
                    Text = opt, Foreground = Brushes.White, FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment   = VerticalAlignment.Center
                };
                card.MouseEnter += (s, e) =>
                    card.BorderBrush = new SolidColorBrush(Color.FromRgb(0xE9, 0x45, 0x60));
                card.MouseLeave += (s, e) =>
                    card.BorderBrush = new SolidColorBrush(Color.FromRgb(0x2D, 0x35, 0x61));
                card.MouseLeftButtonUp += (s, e) =>
                    SetStatus($"Выбран стиль курсора: {opt} (нужны .cur файлы)");
                CursorPanel.Children.Add(card);
            }
        }

        // ─────────────────────────────────────────────────────
        //  Карточки шрифтов
        // ─────────────────────────────────────────────────────
        private void BuildFontCards()
        {
            FontPanel.Children.Clear();
            var fonts = new[] { "Segoe UI", "Calibri", "Arial", "Roboto", "Consolas", "Tahoma" };
            foreach (var fontName in fonts)
            {
                var card = new Border
                {
                    Width = 160, Height = 80, Margin = new Thickness(0, 0, 12, 12),
                    CornerRadius = new CornerRadius(8), Cursor = Cursors.Hand,
                    Background = new SolidColorBrush(Color.FromRgb(0x16, 0x21, 0x3E)),
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0x2D, 0x35, 0x61))
                };
                var panel = new StackPanel { Margin = new Thickness(12), VerticalAlignment = VerticalAlignment.Center };
                panel.Children.Add(new TextBlock
                {
                    Text = "Aa", FontFamily = new FontFamily(fontName),
                    FontSize = 24, Foreground = Brushes.White
                });
                panel.Children.Add(new TextBlock
                {
                    Text = fontName, FontSize = 11,
                    Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x92, 0xA4))
                });
                card.Child = panel;

                card.MouseEnter += (s, e) =>
                    card.BorderBrush = new SolidColorBrush(Color.FromRgb(0xE9, 0x45, 0x60));
                card.MouseLeave += (s, e) =>
                    card.BorderBrush = new SolidColorBrush(Color.FromRgb(0x2D, 0x35, 0x61));
                card.MouseLeftButtonUp += (s, e) =>
                    SetStatus($"Шрифт выбран: {fontName}");
                FontPanel.Children.Add(card);
            }
        }

        // ─────────────────────────────────────────────────────
        //  Загрузка списка музыки
        // ─────────────────────────────────────────────────────
        private void LoadMusicList()
        {
            MusicList.Items.Clear();
            _tracks.Clear();

            string musicDir = GetAssetPath("Music");
            var trackNames = new[] { "Ambient Dark", "Ocean Waves", "Forest Night",
                                     "Night Sky", "Space Drift", "Cyber City",
                                     "Warm Evening", "Ice Crystal" };

            // Добавляем встроенные имена (файлы опциональны)
            foreach (var name in trackNames)
            {
                MusicList.Items.Add("  🎵  " + name);
            }

            // Сканируем папку Music на реальные файлы
            if (Directory.Exists(musicDir))
            {
                foreach (var file in Directory.GetFiles(musicDir, "*.mp3"))
                {
                    _tracks.Add(file);
                }
                foreach (var file in Directory.GetFiles(musicDir, "*.wav"))
                {
                    _tracks.Add(file);
                }
            }

            if (MusicList.Items.Count > 0)
                MusicList.SelectedIndex = 0;
        }

        // ─────────────────────────────────────────────────────
        //  Применение темы
        // ─────────────────────────────────────────────────────
        private void ApplyTheme(AppTheme theme)
        {
            _currentTheme = theme;
            SetStatus($"Применяется тема: {theme.Name}...");

            // Обои
            string wallDir  = GetAssetPath("Wallpapers");
            string wallPath = Path.Combine(wallDir, theme.WallpaperFile);
            if (File.Exists(wallPath))
                SetWallpaper(wallPath);
            else
                SetStatus($"Тема {theme.Name} выбрана. Обои: поместите '{theme.WallpaperFile}' в папку Wallpapers/");

            // Музыка
            string musicDir  = GetAssetPath("Music");
            string trackPath = Path.Combine(musicDir, theme.MusicTrack);
            if (File.Exists(trackPath))
                PlayTrack(trackPath);

            // Обновляем UI
            TxtNowPlaying.Text = theme.Name + " Ambient";
            SetStatus($"✅ Тема «{theme.Name}» применена");
        }

        // ─────────────────────────────────────────────────────
        //  Применение цвета иконок
        // ─────────────────────────────────────────────────────
        private void ApplyIconColor(IconColorOption option)
        {
            var brush = new SolidColorBrush(option.Color);
            IconPreview1.Background = brush;
            IconPreview2.Background = brush;
            IconPreview3.Background = brush;
            IconPreview4.Background = brush;
            SetStatus($"Цвет иконок: {option.Name}");
        }

        // ─────────────────────────────────────────────────────
        //  Установка обоев через WinAPI
        // ─────────────────────────────────────────────────────
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE   = 0x01;
        private const int SPIF_SENDCHANGE      = 0x02;

        private void SetWallpaper(string path)
        {
            try
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path,
                    SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
                SetStatus($"✅ Обои установлены: {Path.GetFileName(path)}");
            }
            catch (Exception ex)
            {
                SetStatus($"❌ Ошибка установки обоев: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────
        //  Музыкальный плеер
        // ─────────────────────────────────────────────────────
        private void PlayTrack(string path)
        {
            try
            {
                _player.Open(new Uri(path, UriKind.Absolute));
                _player.Volume  = SliderVolumeMini.Value / 100.0;
                _player.Play();
                _isPlaying = true;
                BtnPlayPause.Content = "⏸";
                TxtNowPlaying.Text   = Path.GetFileNameWithoutExtension(path);
                SetStatus($"▶ Играет: {Path.GetFileName(path)}");
            }
            catch (Exception ex)
            {
                SetStatus($"❌ Ошибка воспроизведения: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────
        //  Вспомогательные методы
        // ─────────────────────────────────────────────────────
        private string GetAssetPath(string subFolder)
        {
            // Папка рядом с exe или рядом с .csproj при отладке
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, subFolder);
        }

        private void SetStatus(string text)
        {
            TxtStatus.Text = text;
        }

        private void ShowPage(string pageName)
        {
            PageThemes.Visibility    = Visibility.Collapsed;
            PageIcons.Visibility     = Visibility.Collapsed;
            PageWallpaper.Visibility = Visibility.Collapsed;
            PageMusic.Visibility     = Visibility.Collapsed;
            PageCursor.Visibility    = Visibility.Collapsed;
            PageFonts.Visibility     = Visibility.Collapsed;

            switch (pageName)
            {
                case "Themes":    PageThemes.Visibility    = Visibility.Visible; break;
                case "Icons":     PageIcons.Visibility     = Visibility.Visible; break;
                case "Wallpaper": PageWallpaper.Visibility = Visibility.Visible; break;
                case "Music":     PageMusic.Visibility     = Visibility.Visible; break;
                case "Cursor":    PageCursor.Visibility    = Visibility.Visible; break;
                case "Fonts":     PageFonts.Visibility     = Visibility.Visible; break;
            }
        }

        // ─────────────────────────────────────────────────────
        //  Обработчики кнопок меню
        // ─────────────────────────────────────────────────────
        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                ShowPage(tag);
                SetStatus("Раздел: " + btn.Content.ToString().Trim());
            }
        }

        private void BtnApplyAll_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTheme != null)
                ApplyTheme(_currentTheme);
            else
                SetStatus("Сначала выберите тему!");
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _player.Stop();
            _isPlaying           = false;
            BtnPlayPause.Content = "▶";
            TxtNowPlaying.Text   = "Ничего не выбрано";
            SetStatus("Настройки сброшены");
        }

        // ─────────────────────────────────────────────────────
        //  Обработчики плеера
        // ─────────────────────────────────────────────────────
        private void BtnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                _player.Pause();
                _isPlaying           = false;
                BtnPlayPause.Content = "▶";
                SetStatus("⏸ Пауза");
            }
            else
            {
                _player.Play();
                _isPlaying           = true;
                BtnPlayPause.Content = "⏸";
                SetStatus("▶ Воспроизведение");
            }
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_tracks.Count == 0) { SetStatus("Нет треков. Добавьте mp3 в папку Music/"); return; }
            _trackIndex = (_trackIndex - 1 + _tracks.Count) % _tracks.Count;
            PlayTrack(_tracks[_trackIndex]);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_tracks.Count == 0) { SetStatus("Нет треков. Добавьте mp3 в папку Music/"); return; }
            _trackIndex = (_trackIndex + 1) % _tracks.Count;
            PlayTrack(_tracks[_trackIndex]);
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_player == null) return;
            _player.Volume = e.NewValue / 100.0;
            // Синхронизируем оба слайдера
            if (TxtVolumePercent != null)
                TxtVolumePercent.Text = $"{(int)e.NewValue}%";
            if (SliderVolumeMini != null && sender != SliderVolumeMini)
                SliderVolumeMini.Value = e.NewValue;
            if (SliderVolume != null && sender != SliderVolume)
                SliderVolume.Value = e.NewValue;
        }

        private void MusicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Если есть реальные треки — играем
            if (_tracks.Count > 0 && MusicList.SelectedIndex < _tracks.Count)
            {
                _trackIndex = MusicList.SelectedIndex;
                PlayTrack(_tracks[_trackIndex]);
            }
            else
            {
                SetStatus("Добавьте mp3-файлы в папку Music/ для воспроизведения");
            }
        }

        // ─────────────────────────────────────────────────────
        //  Обои: выбор файла и случайные
        // ─────────────────────────────────────────────────────
        private void BtnPickWallpaper_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы|*.*",
                Title  = "Выберите обои"
            };
            if (dlg.ShowDialog() == true)
                SetWallpaper(dlg.FileName);
        }

        private void BtnRandomWallpaper_Click(object sender, RoutedEventArgs e)
        {
            string wallDir = GetAssetPath("Wallpapers");
            if (!Directory.Exists(wallDir)) { SetStatus("Папка Wallpapers/ не найдена"); return; }

            var files = new List<string>();
            files.AddRange(Directory.GetFiles(wallDir, "*.jpg"));
            files.AddRange(Directory.GetFiles(wallDir, "*.png"));
            files.AddRange(Directory.GetFiles(wallDir, "*.bmp"));

            if (files.Count == 0) { SetStatus("Нет изображений в папке Wallpapers/"); return; }

            var rnd  = new Random();
            var pick = files[rnd.Next(files.Count)];
            SetWallpaper(pick);
        }

        // ─────────────────────────────────────────────────────
        //  Добавление своего трека
        // ─────────────────────────────────────────────────────
        private void BtnAddTrack_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Аудио файлы|*.mp3;*.wav;*.wma;*.ogg|Все файлы|*.*",
                Title  = "Выберите аудио файл"
            };
            if (dlg.ShowDialog() == true)
            {
                _tracks.Add(dlg.FileName);
                MusicList.Items.Add("  🎵  " + Path.GetFileNameWithoutExtension(dlg.FileName));
                _trackIndex = _tracks.Count - 1;
                PlayTrack(dlg.FileName);
                SetStatus($"Добавлен трек: {Path.GetFileName(dlg.FileName)}");
            }
        }

        // ─────────────────────────────────────────────────────
        //  Шрифты: слайдер
        // ─────────────────────────────────────────────────────
        private void SliderFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TxtFontSize != null)
                TxtFontSize.Text = $"{(int)e.NewValue}px";
        }

        // ─────────────────────────────────────────────────────
        //  Управление окном (перетаскивание, кнопки)
        // ─────────────────────────────────────────────────────
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            _player?.Stop();
            this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
            => this.WindowState = WindowState.Minimized;

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }
}
