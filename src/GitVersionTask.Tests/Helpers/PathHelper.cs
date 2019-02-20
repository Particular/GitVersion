﻿using System;
using System.IO;
using System.Reflection;

public static class PathHelper
{
    public static string GetCurrentDirectory()
    {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }

    public static string GetTempPath()
    {
        return Path.Combine(GetCurrentDirectory(), "TestRepositories", Guid.NewGuid().ToString());
    }
}