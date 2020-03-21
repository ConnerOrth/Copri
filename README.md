# Copri
**Co**mpiler **Pri**mordial

Compiler
> /kəmˈpʌɪlə/ a program that converts instructions into a machine-code or lower-level form so that they can be read and executed by a computer.

Primordial
> /prʌɪˈmɔːdɪəl/ in the earliest stage of development.

This repo contains **Copri**, a handwritten compiler in C#. It illustrates basic
concepts of compiler construction and how one can tool the language inside of an
IDE by exposing APIs for parsing and type checking.

This compiler uses many of the concepts that you can find in the Microsoft
C# and Visual Basic compilers, code named [Roslyn].

[Roslyn]: https://github.com/dotnet/roslyn

## Inspiration for Copri

This code base was heavily inspired by [Minsk]. (Minsk was written live during streaming by [Immo Landwerth/Terrajobst]. You can watch his amazing recordings
on [YouTube].)

[Minsk]: https://minsk-compiler.net
[Immo Landwerth/Terrajobst]: https://github.com/terrajobst
[YouTube]: https://www.youtube.com/playlist?list=PLRAdsfhKI4OWNOSfS7EUu5GRAVmze1t2y