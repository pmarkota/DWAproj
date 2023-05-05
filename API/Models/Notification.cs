using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class Notification
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ReceiverEmail { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string Subject { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Body { get; set; } = null!;

    public bool IsSent { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SentDate { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}
