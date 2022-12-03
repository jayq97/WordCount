@echo off

title Word Counter batch script
echo Welcome to Word Counter

cd "WordCount\WordCount\bin\Debug\net6.0\"
WordCount.exe \TestFolder .txt

pause