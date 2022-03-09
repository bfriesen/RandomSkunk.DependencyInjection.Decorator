# RandomSkunk.DependencyInjection.Decorator

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

### Fixed

- Make release builds deterministic (needed for source link).

## [1.0.0]

### Added

- Create initial project, solution, and package structures.
- Add `IDecoratingBuilder<TService>` interface.
- Add `AddDecorated`, `AddDecoratedTransient`, `AddDecoratedScoped`, and `AddDecoratedSingleton` extension methods for `IServiceCollection`.
- Add `AddDecorator` extension methods for `IDecoratingBuilder<TService>`.

[Keep a Changelog]: https://keepachangelog.com/en/1.0.0/
[Semantic Versioning]: https://semver.org/spec/v2.0.0.html
[1.0.0]: https://github.com/bfriesen/RandomSkunk.DependencyInjection.Decorator/compare/0909129881ba3a306353a11bd548538bf3122723...v1.0.0
[Unreleased]: https://github.com/bfriesen/RandomSkunk.DependencyInjection.Decorator/compare/v1.0.0...HEAD
