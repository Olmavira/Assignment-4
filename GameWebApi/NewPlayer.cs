
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;

public class NewPlayer
{
    [StringLength(5)]
    public string Name { get; set; }
}