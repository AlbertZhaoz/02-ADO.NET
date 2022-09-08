using System;

namespace Advanced.Models
{
    /// <summary>
    /// 实体
    /// </summary>
    public class People
    {
        public People()
        {
            Console.WriteLine("{0}被创建", this.GetType().FullName);
        }


        public int Id { get; set; }

        //public int Id
        //{
        //    get
        //    {
        //        return this._Id;
        //    }
        //    set
        //    {

        //        this._Id = value;
        //    }
        //} //属性不能存储值

        //public int _Id;

        public string Name { get; set; }

        public int Age { get; set; }

        public string Description;






        public void Show()
        {

        }
    }
}
