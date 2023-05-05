using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class Video
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string Image { get; set; } = null!;

    public int TotalTime { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string StreamingUrl { get; set; } = null!;

    [ForeignKey("VideoId")]
    [InverseProperty("Videos")]
    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    [ForeignKey("VideoId")]
    [InverseProperty("Videos")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
