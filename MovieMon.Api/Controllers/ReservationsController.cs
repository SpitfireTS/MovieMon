using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieMon.Api.Models;

namespace MovieMon.Api.Controllers
{
    public class ReservationsController : ApiController
    {

        // POST /api/reservations
        public HttpResponseMessage<MovieReservationResult> PostReservation(MovieReservation reservation)
        {

            return new HttpResponseMessage<MovieReservationResult>(new MovieReservationResult { ConfirmationMessage = MakeMessage(reservation) }, HttpStatusCode.OK);
        }

        private static string MakeMessage(MovieReservation reservation)
        {
            if (reservation.ProviderName == "Netflix")
            {
                return string.Format("{0} on {1} was added to your Netflix Queue", reservation.Title, reservation.Format);
            }
            return string.Format("{0} on {1} is waiting for your at: ", reservation.Title, reservation.Format);
        }
    }
}
