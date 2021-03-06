# RandomSkunk.DependencyInjection.Decorator

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

## [1.0.1] - 2022-03-22

### Fixed

- Fix missing intellisense in nuget package ([#1]).
- Fix source link in nuget package ([#2]).

## [1.0.0] - 2022-03-09

### Added

- Create initial project, solution, and package structures.
- Add `IDecoratingBuilder<TService>` interface.
- Add `AddDecorated`, `AddDecoratedTransient`, `AddDecoratedScoped`, and `AddDecoratedSingleton` extension methods for `IServiceCollection`.
- Add `AddDecorator` extension methods for `IDecoratingBuilder<TService>`.

[Keep a Changelog]: https://keepachangelog.com/en/1.0.0/
[Semantic Versioning]: https://semver.org/spec/v2.0.0.html
[1.0.0]: ../../compare/0909129881ba3a306353a11bd548538bf3122723...v1.0.0
[1.0.1]: ../../compare/v1.0.0...v1.0.1
[Unreleased]: ../../compare/v1.0.1...HEAD

[#1]: ../../issues/1
[#2]: ../../issues/2
