# Changelog

All notable changes to the `Gpt4All` project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

The focus should be on keeping a human-readable changelog to ease the investigation of recently introduced bugs or undocumented feature.

## [Unreleased]

## [0.7.0] - 2023-06-12

### Added

- Dynamic library loading ([#763](https://github.com/nomic-ai/gpt4all/pull/763))

### Fixed

- Aligned the bindigs to work with breaking changes in the backend

## [0.6.1] - 2023-06-01

### Added

- Added support to integrate with the .NET logging API ([#714](https://github.com/nomic-ai/gpt4all/pull/714))

### Changed

- added an optional `Microsoft.Extensions.Logging.ILogger` parameter to the ctors of `Gpt4All`, `Gpt4AllModelFactory` and `LLModel`

## [0.6.0] - 2023-05-29

### Added

- Custom prompt formatting `Gpt4All.IPromptFormatter` ([#712](https://github.com/nomic-ai/gpt4all/pull/712))

### Changed

- The default prompt format now defaults to the implementation in `Gpt4All.DefaultPromptFormatter`

## [0.5.0] - 2023-05-22

### Added

- First alpha release of the C# bindings ([#650](https://github.com/nomic-ai/gpt4all/pull/650))