//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tea.ModelSQL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Delivery = new HashSet<Delivery>();
            this.ProductInDelivery = new HashSet<ProductInDelivery>();
            this.ProductInSale = new HashSet<ProductInSale>();
        }
    
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public byte[] Image { get; set; }
        public int Price { get; set; }
        public string Information { get; set; }
        public int IdUnit { get; set; }
        public int IdForm { get; set; }
        public int IdCountry { get; set; }
        public int IdView { get; set; }
        public int IdCategory { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Delivery> Delivery { get; set; }
        public virtual Form Form { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual View View { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInDelivery> ProductInDelivery { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInSale> ProductInSale { get; set; }
    }
}
