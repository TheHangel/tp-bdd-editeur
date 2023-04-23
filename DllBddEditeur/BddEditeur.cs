using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BddediteurContext;

namespace DllBddEditeur
{
    public class BddEditeur
    {
        private BddediteurDataContext bdd = null;
        public BddEditeur()
        {
            try
            {
                bdd = new BddediteurDataContext();
            }
            catch
            {
                throw new Exception("problème pour instancier l'attribut bdd");
            }
        }
        public BddEditeur(string serveurIp, string port, string user, string mdp)
        {
            try
            {
                //connectionString = "User Id=AdminEditeur;Password=@Password1234!;Database=bddediteur;Persist Security Info=True"
                bdd = new BddediteurDataContext("User Id=" + user + ";Password=" + mdp + ";Host=" + serveurIp + ";Port=" + port + ";Database=BddEditeur;Persist Security Info=True");
            }
            catch
            {
                throw;
            }
        }
        public List<Bookauthor> getallAuthors()
        {
            try
            {
                return bdd.Bookauthors.ToList();
            }
            catch { return new List<Bookauthor>(); }
        }
        public List<Booklist> getallBooks()
        {
            try
            {
                return bdd.Booklists.ToList();
            }
            catch { return new List<Booklist>(); }
        }

        public List<User> getallUsers()
        {
            try
            {
                return bdd.Users.ToList();
            }
            catch { return new List<User>(); }
        }

        public List<Bookprice> GetBookprices()
        {
            try
            {
                return bdd.Bookprices.ToList();
            }
            catch { return new List<Bookprice>(); }
        }
        public bool authorExiste(string isbn)
        {
            try
            {
                Bookauthor aut = bdd.Bookauthors.Where(s => s.ISBN.ToLower() == isbn.ToLower()).FirstOrDefault();
                if (aut != null)
                    return true;
                return false;
            }
            catch { throw; }
        }

        public bool userExiste(string login)
        {
            try
            {
                User user = bdd.Users.Where(s => s.Login == login).FirstOrDefault();
                if (user != null)
                    return true;
                return false;
            }
            catch { throw; }
        }

        public Bookauthor getAuthor(string isbn)
        {
            try
            {
                Bookauthor aut = bdd.Bookauthors.Where(s => s.ISBN.ToLower() == isbn.ToLower()).FirstOrDefault();
                return aut;
            }
            catch { throw; }
        }

        public User userConnecte(string login, string mdp)
        {
            try
            {
                User user = bdd.Users.Where(s => s.Login.ToLower() == login.ToLower() && s.Mdp == mdp).FirstOrDefault();
                return user;
            }
            catch { throw; }
        }

        public bool addAuthor(string n, string p, string isb)
        {
            bool Result;
            try
            {
                Bookauthor aut = new Bookauthor();
                aut.FirstName = n;
                aut.LastName = p;
                aut.ISBN = isb;
                bdd.Bookauthors.InsertOnSubmit(aut);
                bdd.SubmitChanges();
                Result = true;
            }
            catch { Result = false; }
            return Result;
        }
        public bool addUser(string n, string p, string lo, string mdp, bool auto)
        {
            bool Result;
            try
            {
                User u = new User();
                u.Nom = n;
                u.Prenom = p;
                u.Login = lo;
                u.Mdp = mdp;
                u.Autorisation = auto;
                bdd.Users.InsertOnSubmit(u);
                bdd.SubmitChanges();
                Result = true;
            }
            catch { Result = false; }
            return Result;
        }
        public bool AddBook(string isb, string titre, DateTime dateP)
        {
            bool Result;
            try
            {
                Booklist livre = new Booklist();
                livre.ISBN = isb;
                livre.Title = titre;
                livre.PublicationDate = dateP;
                bdd.Booklists.InsertOnSubmit(livre);
                bdd.SubmitChanges();
                Result = true;
            }
            catch { Result = false; }
            return Result;
        }

        public Bookprice getBookPrice(string isbn)
        {
            return bdd.Bookprices.Where(s => s.ISBN == isbn).FirstOrDefault();
        }


        public List<Bookprice> getCurrencies(string isbn)
        {
            return bdd.Bookprices.Where(s => s.ISBN == isbn).ToList();
        }

        public Bookprice getPriceFromCurrency(string isbn, string curr)
        {
            return bdd.Bookprices.Where(s => s.ISBN == isbn && s.Currency == curr).FirstOrDefault();
        }

        public bool UpdateUser(User user, string nom, string prenom, string mdp, bool autorisation)
        {
            bool result;
            try
            {
                if (user != null)
                {
                    user.Nom = nom;
                    user.Prenom = prenom;
                    user.Mdp = mdp;
                    user.Autorisation = autorisation;
                    bdd.SubmitChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public List<Booklist> getBooksFromAuthor(string isbn)
        {
            var query = from book in bdd.Booklists
                        join author in bdd.Bookauthors on book.ISBN equals author.ISBN
                        where author.ISBN == isbn
                        select book;

            return query.ToList();
        }

        public bool deleteUser(User user)
        {
            bool Result;
            try
            {
                bdd.Users.DeleteOnSubmit(user);
                bdd.SubmitChanges();
                Result = true;
            }
            catch { Result = false; }
            return Result;
        }

        public bool deleteUBook(Booklist book)
        {
            bool Result;
            try
            {
                bdd.Booklists.DeleteOnSubmit(book);
                bdd.SubmitChanges();
                Result = true;
            }
            catch { Result = false; }
            return Result;
        }

    }
}
