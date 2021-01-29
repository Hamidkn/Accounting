using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Services
{
  // we create a generic repository, which when an instance will create for any table or any class that table or class will be instead of TEntity.
  // when an instance of any classes like customers or users will create, The TEntity will change to that instance.
  public class GenericRepository<TEntity> where TEntity: class
  {
    private Accounting_DBEntities _db;
    private DbSet<TEntity> _dbSet;

    public GenericRepository(Accounting_DBEntities db)
    {
      _db = db;
      _dbSet = _db.Set<TEntity>(); 
    }

    //IEnumerable can change to all it's children like list , ....
    public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity,bool>> where = null)
    {
      // Lazy Load Operation: in here query does not create until it called.
      IQueryable<TEntity> query = _dbSet;

      if (where == null)
      {
        query = query.Where(where);
      }
      // here that the query is called, line 28 will be created and will be run.
      return query.ToList();
    }
    public virtual void Insert(TEntity entity)
    {
      _dbSet.Add(entity);
    }

    //when we do not know the type of the Id in each table , we put object and that will change to the type of Id in each table
    // the Virtual will make us to override this method in other classes
    public virtual TEntity GetByID(object Id)
    {
      return _dbSet.Find(Id);
    }

    public virtual void Update(TEntity entity)
    {
      _dbSet.Attach(entity);
      _db.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
      if (_db.Entry(entity).State == EntityState.Detached)
      {
        _dbSet.Attach(entity);
      }

      _dbSet.Remove(entity);
    }

    public virtual void Delete(object Id)
    {
      var entity = GetByID(Id);
      Delete(entity);
    }
  }
}
