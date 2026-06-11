# Synth

A simple CLI-based music player built in C# using **WMPLib (Windows Media Player API)**.

---

# WMPLib

WMPLib is a .NET wrapper around the Windows Media Player engine.
It lets you play audio files.

It  kinda like using Windows Media Player behind the scenes, but controlling it with code.


---

# How WMPLib Works

Instead of handling audio decoding it, you can:

1. Create a media player instance
2. Load a file
3. Control playback (play, pause, stop)
4. Read metadata (duration, title, etc.)

---

# Essential Classes & Interfaces

## `WindowsMediaPlayer`

Main class that controls everything.

```csharp
var player = new WindowsMediaPlayer();
```

* Loads audio files
* Controls playback
* Gives access to media info

---

## `IWMPControls`

Controls playback actions.

```csharp
player.controls.play();
player.controls.pause();
player.controls.stop();
```

---

## `IWMPMedia`

Represents a loaded media file.

```csharp
IWMPMedia media = player.newMedia("song.mp3");
double duration = media.duration;
```

Provides:

* Duration (in seconds)
* Metadata (artist, title, etc.)

Note: Duration may not be immediately available when loading.

---

## `IWMPPlaylist`

Used for handling multiple songs.

* Add/remove tracks
* Loop through songs

*(Not heavily used in this project, since we manage songs manually.)*

---

# ▶Basic Example

```csharp
using WMPLib;

var _player = new WindowsMediaPlayer();

// Load a file
_player.URL = @"C:\Music\song.mp3";

// Play it
_player.controls.play();
```

---

# Getting Song Duration

```csharp
var _player = new WindowsMediaPlayer();
IWMPMedia media = _player.newMedia(@"C:\Music\song.mp3");

double duration = media.duration; // in seconds
```

Convert to readable format:

```csharp
TimeSpan time = TimeSpan.FromSeconds(duration);
Console.WriteLine(time.ToString(@"mm\:ss"));
```

---

# Important Notes

* WMPLib is Windows-only
* Media loading is asynchronous
* Duration might return `0` if accessed too early
* Works best with local files (not streaming)

---

