﻿using SmartParkingAbstract.ViewModels.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Data
{
    public delegate IDataParsingService DataParsingResolver(string key);
    public interface IDataParsingService
    {
        /// <summary>
        /// Open stream for reader
        /// </summary>
        /// <param name="stream"></param>
        public void Open(Stream stream);

        /// <summary>
        /// Close and release object
        /// </summary>
        public void Close();

        /// <summary>
        /// Parsing Data
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="parsingOption">Option</param>
        /// <returns></returns>
        public IEnumerable<T> ParseData<T>(ParsingOption parsingOption) where T : new(); 
    }
}
