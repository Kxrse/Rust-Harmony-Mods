@echo off
cd /d "C:\Users\Kxrse\Desktop\Rust\HarmonyModRepo"
git pull --rebase origin main
git add .
set /p msg="Commit message: "
git commit -m "%msg%"
git push
pause