﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PracaDyplomowa.Models;

namespace PracaDyplomowa.ViewsModel
{
    public class AddEventVM
    {
        public int EventId { get; set; }
        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa za długa")]
        public string Name { get; set; }
        [StringLength(300, ErrorMessage = "Krutki opis jest za długa")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany")]
        [StringLength(1000, ErrorMessage = "Opis za długa")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Lokalizacja jest wymagana")]
        [StringLength(200, ErrorMessage = "Miejsce jest za długe")]
        public string Place { get; set; }
        //[Required(ErrorMessage = "Data rozpoczecia jest wymagana")]
        [DataType(DataType.DateTime,ErrorMessage = "Proszę podać popraną datę rozpoczecia")]
        public DateTime? DateStart { get; set; }
        //[Required(ErrorMessage = "Data zakonczenia jest wymagana")]
        [DataType(DataType.DateTime, ErrorMessage ="Proszę podać popraną datę zakończenia" )]
        public DateTime? DateEnd { get; set; }
        public string UserName { get; set; }
        public FirmAccount FirmAccount { get; set; }
        public List<Publication> Publications { get; set; }
        public string error { get; set; }
    }
}
