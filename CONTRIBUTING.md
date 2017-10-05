# Contributing

## Welcome!
Hello. My name is Nelson and MushROMs has been a passion project of mine for many years. I currently work three jobs that I love and am working on going to graduate school for astrophysics. I therefore have very little time to commit to my little hobbies and am thus very greatful that you are interested in contributing to MushROMs. It really motivates me to know other people share my interests. Before getting started, however, I would really like you to read over this document to see how I like contributions to be submitted. It is my belief that these guidelines will help all of us best understand each other and collaborate together

## Table of Contents
* [How to file a bug report](#how-to-file-a-bug-report)
* [How to suggest a new feature](#how-to-suggest-a-new-feature)
* [How to set up your environment and run tests](#how-to-set-up-your-environment-and-run-tests)
* [The types of contributions we're looking for](#the-types-of-contributions-were-looking-for)
* [Our vision for the project](#our-vision-for-the-project)
* [Code styles](#code-styles)
    - [Commit messages](#commit-messages)

## How to file a bug report
To file a bug report, visit the [Issues](https://github.com/bonimy/MushROMs/issues) page, and search if the bug has already been reported. If it has not, then open a new issue, giving a short, descriptive title explaining the bug. From there, you will get a template file that outlines how to describe your bug and what information we are looking for when you submit it.

## How to suggest a new feature
Same as filing a bug report, open a new [Issue](https://github.com/bonimy/MushROMs/issues) and follow the guidelines in the template.

## How to set up your environment and run tests
> Presently, the [Visual Studio 2017 IDE](https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes) is the only supported environment for MushROMs. Users are encouraged to suggest new environments in our [Issues](https://github.com/bonimy/MushROMs/issues) section.

Refer to the [README](README.md#installation) on the front page for a complete setup instructions.

## The types of contributions we're looking for
Refer to our [VISION](VISION.md) file for a better idea of what exactly MushROMs is and how I want it to get there. To summarize the points it makes, the following contributions are greatly supported:
* Unit/Benchmark tests
* Source code documentation
* Bug fixes
* Cross-platform support
* Cross-IDE support
* Game editing utilities (e.g. Music editor, 3D object editor, etc.)
* Issue closers

Below are some things we think wouldn't be helpful for contributions
* Opening an issue that says "You should add support for _Game X_ and having no code documentation for the game. I would love to add support for _Game X_ but opening an issue for that with no extra information wouldn't do much for anyone.
* Saying "I can't build on my computer" without providing details on what you tried, citing the README and the CONTRIBUTING file.
* Submitting a pull request with no description of what the code does. Even if it's a typo fix, mention that it's a typo fix.
* Submitting a pull request without opening an Issue first. Every pull request should be a solution to an existing issue. For very trivial things like typos, this is an exception. However, one line bug fix patches should still have an issue open to address the bug.
* Submitting a bug fix without an accompanying unit test to catch the bug from now on (except in cases of bugs that were typos e.g. `if (x = true)` to `if (x == true)`.
* Requesting a feature that is far outside the scope of our VISION (e.g. _Make an editor for Super Mario 64_).

## Our vision for the project
> MushROMs is intended to be the core for Super NES game editors.

Refer to [VISION](VISION.md) for a complete description of our project's vision.

## Code styles
Refer to our [CODE STYLES](CODE_STYLES.md) guide for an in-depth discussion on how to write your code for this project.

### Commit messages
[This](https://chris.beams.io/posts/git-commit/) article does a great job describing the value of a formatted git commit message. The things to take away from it are
1. [Separate subject from body with a blank line](https://chris.beams.io/posts/git-commit/#separate)
2. [Limit the subject line to 50 characters](https://chris.beams.io/posts/git-commit/#limit-50)
3. [Capitalize the subject line](https://chris.beams.io/posts/git-commit/#capitalize)
4. [Do not end the subject line with a period](https://chris.beams.io/posts/git-commit/#end)
5. [Use the imperative mood in the subject line](https://chris.beams.io/posts/git-commit/#imperative)
6. [Wrap the body at 72 characters](https://chris.beams.io/posts/git-commit/#wrap-72)
7. [Use the body to explain _what_ and _why_ vs. _how_](https://chris.beams.io/posts/git-commit/#why-not-how)

Try to comply to these rules as much as possible.
