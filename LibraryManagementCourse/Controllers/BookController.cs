﻿using LibraryManagementCourse.Data.Interfaces;
using LibraryManagementCourse.Data.Model;
using LibraryManagementCourse.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementCourse.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        [Route("Book")]
        public IActionResult List(int? authorId, int? borrowerId)
        {
            if(authorId == null && borrowerId == null)
            { // show all books
                var books = _bookRepository.GetAllWithAuthor();

                //check books
                return CheckBooks(books);
            }
            else if(authorId != null)
            { // filter by author Id
                var author = _authorRepository.GetWithBooks((int)authorId);

                //check author books
                if(author.Books.Count() == 0)
                {
                    return View("AuthorEmpty", author);
                }
                else
                {
                    return View(author.Books);
                }
            }
            else if(borrowerId != null)
            { // filter by borrower Id
                var books = _bookRepository.FindWithAuthorAndBorrower(book => book.BorrowerId == book.BorrowerId);

                //check borrower books
                return CheckBooks(books);
            }
            else
            { // throw exception
                throw new Exception();
            }
        }

        public IActionResult CheckBooks(IEnumerable<Book> books)
        {
            if (books.Count() == 0)
            {
                return View("Empty");
            }
            else
            {
                return View(books);
            }
        }

        public IActionResult Create()
        {
            var bookVM = new BookViewModel() { Authors = _authorRepository.GetAll() };
            return View(bookVM);
        }

        [HttpPost]
        public IActionResult Create(BookViewModel bookViewModel)
        {
            _bookRepository.Create(bookViewModel.Book);
            return RedirectToAction("List");
        }

        public IActionResult Update()
        {
            var bookVM = new BookViewModel() { Authors = _authorRepository.GetAll() };
            return View(bookVM);
        }

        [HttpPost]
        public IActionResult Update(BookViewModel bookViewModel)
        {
            _bookRepository.Update(bookViewModel.Book);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            _bookRepository.Delete(book);

            return RedirectToAction("List");
        }
    }
}
