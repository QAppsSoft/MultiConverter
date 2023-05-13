# MultiConverter

[![.NET target](https://img.shields.io/badge/dynamic/xml?color=%23512bd4&label=target&query=%2F%2FTargetFramework%5B1%5D&url=https%3A%2F%2Fraw.githubusercontent.com%2FQAppsSoft%2FMultiConverter%2Fmain%2Fsrc%2FMultiConverter.Desktop%2FMultiConverter.Desktop.csproj)](https://dotnet.microsoft.com/download)
[![Github Issues](https://img.shields.io/github/issues/QAppsSoft/MultiConverter)](https://github.com/QAppsSoft/MultiConverter/issues)
![Forks](https://img.shields.io/github/forks/QAppsSoft/MultiConverter)
![Stars](https://img.shields.io/github/stars/QAppsSoft/MultiConverter)
![License](https://img.shields.io/github/license/QAppsSoft/MultiConverter)

![GitHub repo size](https://img.shields.io/github/repo-size/QAppsSoft/MultiConverter?color=%234682B4)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/QAppsSoft/MultiConverter?color=%23483D8B)
[![wakatime](https://wakatime.com/badge/user/1b3378e8-624e-4952-b453-87d2dfdbc51e/project/23e5bf26-2c9a-4516-beb0-bfbab9b4e252.svg)](https://wakatime.com/badge/user/1b3378e8-624e-4952-b453-87d2dfdbc51e/project/23e5bf26-2c9a-4516-beb0-bfbab9b4e252)

MultiConverter aims to be a full replacement for a closed source video converter used in my daily job (developed by me). I will reuse some parts of the original code but mostly will be new code. This was my first attempt in WPF and C#.

### TODO

- [X] Video loading and analysis
  - [ ] Black border detection


- [ ] Editor
  - [ ] Audio language selection
    - [ ] External audio loading
  - [ ] Subtitle language selection
    - [ ] External subtitle loading
  - [ ] Conversion Preset selection
    - [ ] Select preset for all
    - [ ] Select preset per video file


- [ ] Conversion queue
  - [ ] Priority
  - [ ] Status
  - [ ] Cancellation
  - [ ] File remove and Queue cleanup
  - [ ] Queue search and filtering
  - [ ] Per file action
    - Play original/converted video (if exist)
    - Open original/converted containing folder
    - Delete original/converted video filter
  - [ ] Compare video info
    - Duration
    - Size
    - Format
    - Others
  - [ ] Failed conversion reasons


- [ ] Remote and/or local conversion
  - [ ] Limit parallel conversions
  - [ ] Remote conversion (multi server)
    - [ ] Remote server auto detection
    - [ ] Remote server manual attach
    - [ ] Server status
    - [ ] Access server log
    - [ ] Access server stats
    - [ ] General and/or per server config
    - [ ] Remote server shutdown after conversion
  - [ ] Local only conversion


- [ ] Conversion Presets
  - [ ] Default preset selection
  - [X] Video format
    - Output format
    - Video codec
    - Bitrate
    - Resolution
    - Aspect ratio
    - Frames per second
  - [X] Audio format
    - Audio codec
    - Bitrate
    - Sample rate
  - [ ] Advanced
    - [ ] After conversion action
      - Nothing
      - Delete
      - Execute command
    - [ ] Advanced parameters
    - [ ] Video filters
      - [ ] Auto crop black borders
    - [ ] Audio filters
      - [ ] Audio normalization
      - [ ] Fix mono audio (this is for personal use)
  - [ ] Export and import
  - [X] Preset saving
  - [ ] Output file naming convention


- [X] App Settings
  - [X] Dark/Light mode
  - [X] App language selection
  - [X] Temporal conversion folder selection
  - [X] Supported video files
  - [X] File loading filter
  
  
- [ ] Plugins system, should support UI and non UI plugins (Some ideas)
  - [ ] Plugins control, enable and disable plugins
  - [ ] Support remote conversion (Multi PC conversion)
  - [ ] Remote control using Telegram bot platform
  - [ ] Support more custom audio/video filters (Maybe)
  - [ ] Support external file loading filtering
  - [ ] Output file naming convention support
  
