using SudokuBasis;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SudokuWeb.Controllers
{
    public class SudokuController : Controller
    {

        public ActionResult Index()
        {
            ISudoku sudoku;

            if (Session["Sudoku"] == null)
            {
                sudoku = new SudokuBasis.Sudoku();
                sudoku.Create();

                List<int> unadaptableIndexes = new List<int>(); 

                int i = 0;
                for(short y =1; y < 10; y++)
                {
                    for (short x = 1; x < 10; x++)
                    {
                        short val = sudoku.Get(x, y);
                        if (val > 0)
                            unadaptableIndexes.Add(i);
                        i++;
                    }
                }

                Session.Add("UnadaptableIndexes", unadaptableIndexes);
                Session.Add("Sudoku", sudoku);
            }
                
            sudoku = (ISudoku)Session["Sudoku"];

            return View(sudoku);
        }
        public ActionResult Complete()
        {
            return View();
        }

        public ActionResult NewGame()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult PlaceNumber(short numX, short numY, short num)
        {
            ISudoku sudoku = (ISudoku)Session["Sudoku"];

            sudoku.Set(numX, numY, num, true);

            if (sudoku.IsCompleted())
                return RedirectToAction("Complete");

            return RedirectToAction("Index");
        }

        public ActionResult Cheat()
        {
            ISudoku sudoku = (ISudoku)Session["Sudoku"];
            sudoku.Cheat();

            return RedirectToAction("Index");
        }

        public ActionResult Hint()
        {
            ISudoku sudoku = (ISudoku)Session["Sudoku"];

            short x, y, value;
            int succeeded;

            sudoku.Hint(out x, out y, out value, out succeeded);

            if (Convert.ToBoolean(succeeded))
                ViewBag.Hint = String.Format("X: {0}, Y: {1} Waarde: {2}", x, y, value);

            return View("Index", sudoku);

        }

    }
}