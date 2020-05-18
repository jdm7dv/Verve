/// Build Tool License
/// 
/// Copyright (c) 2016 Jonathan Moore
///
/// This software is provided 'as-is', without any express or implied warranty. 
/// In no event will the authors be held liable for any damages arising from 
/// the use of this software.
/// 
/// Permission is granted to anyone to use this software for any purpose, 
/// including commercial applications, and to alter it and redistribute it 
/// freely, subject to the following restrictions:
///
/// 1. The origin of this software must not be misrepresented; 
/// you must not claim that you wrote the original software. 
/// If you use this software in a product, an acknowledgment in the product 
/// documentation would be appreciated but is not required.
/// 
/// 2. Altered source versions must be plainly marked as such, 
/// and must not be misrepresented as being the original software.
///
///3. This notice may not be removed or altered from any source distribution.
/// 
using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Exceptions;

namespace MyBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = MyBuildAsync(args);
            t.Wait();                       
        }

        public async static Task<string> MyBuildAsync(string[] valParameter)
        {
            if (valParameter.Length == 1)
            {
                try
                {
                    Project prj = new Project(valParameter[0]);
                    prj.Build();
                    await Task.Delay(100);
                    Thread.Sleep(1000);
                }
                catch (InvalidProjectFileException e)
                {
                    Console.WriteLine("Project file was invaild" + e.Message);
                }
            }
            else if (valParameter.Length == 0)
            {
                Console.WriteLine("Project file was not found");
            }
            return "project built";
            
        }   
      }
        
    }

