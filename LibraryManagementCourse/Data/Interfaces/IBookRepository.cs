﻿using LibraryManagementCourse.Data.Model;
using System;
using System.Collections.Generic;

namespace LibraryManagementCourse.Data.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetAllWithAuthor();

        IEnumerable<Book> FindWithAuthor(Func<Book, bool> predicate);

        IEnumerable<Book> FindWithAuthorAndBorrower(Func<Book, bool> predicate);
    }
}
