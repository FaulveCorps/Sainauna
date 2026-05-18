# Sainauna

Sainauna is a .NET MAUI Android app for learning Baybayin through guided lessons, flashcards, quizzes, reading practice, and writing exercises.

## Features

- Browse the Baybayin alphabet with grouped character views
- Practice with memorize and quiz modes
- Convert Latin text to Baybayin and test reading skills
- Track progress across the learning flow
- Unlock writing exercises as progress increases

## Tech Stack

- .NET MAUI
- C#
- XAML

## Project Structure

- `/MainPage.xaml` - home screen and onboarding tutorial
- `/BaybayinPage.xaml` - alphabet reference
- `/LearnPage.xaml` - learning hub
- `/MemorizePage.xaml` - flashcard practice
- `/QuizPage.xaml` - quiz mode
- `/TestPage.xaml` - conversion and reading practice
- `/WritePage.xaml` - writing exercises
- `/ProgressService.cs` - shared progress tracking

## Prerequisites

- .NET SDK `10.0.202` as specified in `/global.json`
- .NET MAUI Android workload

## Build

```bash
dotnet build Sainauna.sln
```

## Notes

- The current project targets `net10.0-android`.
- The repository currently ships only the application source and shared resources tracked in git.
