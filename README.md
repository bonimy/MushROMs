# MushROMs
Super Nintendo game editing libraries and tools written in C# .NET.

This project is still in "upload" phase. That means it has no code hosted yet. We are currently working on establishing proper documentation so that everything uploaded is understood for new contributors and old.

# Table of Contents
* [What is MushROMs?](#what-is-mushroms)
* [Installation](#installation)
    * [Visual Studio](#visual-studio)
* [History](#history)
* [Contributions](#contributions)
* [Credits](#credits)
* [License](#license)

# What is MushROMs?
MushROMs started as a basic level editor for Super Mario All-Stars for the SNES. In time, it grew to being a generic editor for any tile-based layout game (NES, SNES, Genesis, etc.). However, most of the work currently emphasizes SNES games. What it can ultimately do will come down to what the contributors want it to do.

# Installation
Presently, the [Visual Studio 2017 IDE](https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes) is the only supported environment for MushROMs. Users are encouraged to suggest new environments in our [Issues](https://github.com/bonimy/MushROMs/issues) section.

## Visual Studio
* Get the [latest](https://www.visualstudio.com/downloads/) version of Visual Studio. At the time of writing this, it should be Visual Studio 2017. You have three options: [Community, Professional, and Enterprise](https://www.visualstudio.com/vs/compare/). Any of these three are fine. The collaborators presently build against community since it is free. See that you meet the [System Requirements](https://www.visualstudio.com/en-us/productinfo/vs2017-system-requirements-vs) for Visual Studio for best interaction.
* When installing Visual Studio (or if you've already installed but missed these components, go to the installer),
    - Under the Workloads tab, select **.NET desktop development**
    - Under the Individual Components tab, select .NET Framework 4.7 SDK and .NET Framework 4.7 targeting pack if they weren't already selected.
    - Under the _Code Tools_ section (still in Individual Components tab), select **Git for Windows** and **GitHub extension for Visual Studio**.
* Click Install and let the installer do it's thing.
* Clone our repository and open [MushROMs.sln](MushROMs.sln) in Visual Studio.
* Hit `F5` to Build and Run and you should be all set!

# History
MushROMs has (for better or worse) gone through many revisions. There have been many major style changes requiring large overhauls and usually ending up in complete rewrites. The old versions of MushROMs still exist and are uploaded on GitHub for archival purposes.
* [2011](https://github.com/bonimy/MushROMs-2011-Archive)
* [2012](https://github.com/bonimy/MushROMs-2012-Archive)
* [2014](https://github.com/bonimy/MushROMs-2014-Archive)
* [2015](https://github.com/bonimy/MushROMs-2015-Archive)
* [2016](https://github.com/bonimy/MushROMs-2016-Archive)
* [2017](https://github.com/bonimy/MushROMs-2017-Archive)

# Contributions
Do you want to add a feature, report a bug, or propose a change to MushROMs? That's awesome! First, please refer to our [Contributing](CONTRIBUTING.md) file. We use it in hopes having the best working environment we can.

# Credits
Major contributors to MushROMs
* [Nelson Garcia](https://github.com/bonimy)

# License
MushROMs: Super Nintendo game editing libraries and tools
Copyright (C) 2017 Nelson Garcia

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program. If not, see http://www.gnu.org/licenses/.
