# Synth
Music program created in Visual Studio using C# and CLI.

# WMPLib Essential Classes & Interfaces
- WindowsMediaPlayer: The primary entry-point class to instantiate a standalone, programmatic player without a UI.
- IWMPControls / IWMPControls3: Controls playback functionality like play(), stop(), pause(), fastForward(), and seeking via currentPosition.
- IWMPMedia: Exposes metadata and properties (e.g., duration, author, track name) for individual media items.
- IWMPPlaylist: Allows you to manage lists of media files and cycle through tracks.
 
 ### Example of usage
 ```C#
WindowsMediaPlayer _player = new WMPLib.WindowsMediaPlayer();
_player.URL = @"C:\Path\To\Your\MediaFile.mp3";
_player.controls.play();
 ```