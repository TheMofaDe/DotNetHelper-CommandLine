@echo off
powershell -ExecutionPolicy ByPass -NoProfile -command "& "./build.ps1" -target Generate-Docs %*"