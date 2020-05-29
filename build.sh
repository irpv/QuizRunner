#!/bin/bash
sudo apt install nuget
sudo apt install mono-runtime
sudo apt-get install nunit-console
nuget restore QuizRunner.sln
msbuild /p:Configuraton=Release QuizRunner.sln
cd QuizRunner/bin/Release
mono QuizRunner.exe
