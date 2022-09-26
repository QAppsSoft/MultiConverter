# MultiConverter

<a href="https://dotnet.microsoft.com/download" alt=".NET target"><img alt=".NET target" src="https://img.shields.io/badge/dynamic/xml?color=%23512bd4&label=target&query=%2F%2FTargetFramework%5B1%5D&url=https%3A%2F%2Fraw.githubusercontent.com%2FQAppsSoft%2FMultiConverter%2Fmain%2Fsrc%2FMultiConverter.Desktop%2FMultiConverter.Desktop.csproj&logo=.net" title="Go To .NET Download"></a>

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
  - [ ] Video format
    - Output format
    - Video codec
    - Bitrate
    - Resolution
    - Aspect ratio
    - Frames per second
  - [ ] Audio format
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
  - [ ] Preset saving
  - [ ] Output file naming convention


- [ ] App Settings
  - [ ] Dark/Light mode
  - [ ] App language selection
  - [ ] Temporal conversion folder selection
  - [ ] Supported video files
  - [ ] File loading filter
