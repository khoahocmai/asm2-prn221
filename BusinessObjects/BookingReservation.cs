﻿using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class BookingReservation
{
    public int BookingReservationId { get; set; }

    public DateOnly? BookingDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public int CustomerId { get; set; }

    public byte? BookingStatus { get; set; } // 1 là pending, 2 là finish

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Customer Customer { get; set; } = null!;
}
