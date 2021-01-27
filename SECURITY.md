# Security Policy

## Supported Versions

| Version | Supported          |
| ------- | ------------------ |
| 1.0.0   | :white_check_mark: |

## Reporting a Vulnerability

To report a security issue simply file an issue in this repository.

Expect security issues to be fixed when a patch to it is released on nuget as soon as possible.

Do not expect that every security issue will be accepted, they will be accepted only if:
- The fix does not break ABI compatibility for the major.minor release.
- The fix does not introduce new public api functions to the major.minor release after intially being released.
- The fix does not break how an entire project operates (like how zlib compresses/decompressess data)/
- When an repository admin says it's ok to fix it and slightly break any of the above.
