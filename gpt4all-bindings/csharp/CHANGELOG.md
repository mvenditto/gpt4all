# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

> **Note** **for Maintainers**
> 
> The focus should be on human-readable changelogs for easy evaluation of recently introduced bugs or (undocumented) features.

## [Unreleased]

### Added

  - Chat abstractions:
    - `IChatConversation`
    - `ChatMessage`
    - `ChatRole`
  - Chat API:
    - `IChatCompletion`
  - Extensions:
    - `IGpt4AllModelExtension::RegenerateChatResponse`

### Changed

  - `ILLModelPrompt::Prompt` now returns a `PromptResult` containng token usage for the generated text.

### Breaking Changes

  - `IGtp4AllModel` interface now inherits also `ITextCompletion`
  - added `IPromptFormatter::FormatChatPrompt`
  - added property `IPredictionResult::Usage` (of type `PredictionInfo`) 

## [0.6.1] - 2023-06-01 ([8e89ceb](https://github.com/nomic-ai/gpt4all/commit/8e89ceb54bf6bded5ac48a9e7405a5e2b31921f9))

### Added

  - .NET logging integration

## [0.6.0] - 2023-05-29 ([9eb81cb](https://github.com/nomic-ai/gpt4all/commit/9eb81cb5492213f2897020b0b73d5256b665e1e3))

### Added

  - Introduced the possibility to customize how the a prompt gets formatted before being fed to the model (`IPromptFormatter`)

## [0.5.0] - 2023-05-22 ([8119ff4](https://github.com/nomic-ai/gpt4all/commit/8119ff4df0a99bde44255db2b8c7290b5582ac2b))

### Added

  - First version of the C# bindings
