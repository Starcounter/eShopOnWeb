import scala.concurrent.duration._

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import scala.util.Random

class EShopNova extends Simulation {

    //val url = "http://52.138.203.219:5106"
        val httpProtocol = http
                .baseURL(url)
                .acceptHeader("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8")
                .acceptEncodingHeader("gzip, deflate")

    val customFeeder = new Feeder[String] {
        import scala.util.Random

      private val RNG = new Random

      private def randInt(a: Int, b: Int) = RNG.nextInt(b - a) + a

      // always return true as this feeder can be polled infinitively
      override def hasNext = true

      override def next: Map[String, String] = {
        val email = scala.math.abs(java.util.UUID.randomUUID.getMostSignificantBits) + "_gatling@dontsend.com"

        Map(
          "email" -> email
        )
      }
    }

        val scn = scenario("EShopNova")
                .exec(http("rProductList")
                        .get("/"))
                .pause(5)
                .exec(http("SignIn")
                        .get("/Account/SignIn"))
                .pause(1)
                .exec(http("Register")
                        .get("/Account/Register"))
                .pause(3)
        .feed(customFeeder)
                .exec(http("RegisterData")
                        .post("/Account/Register")
                        .formParam("Email", "${email}")
                        .formParam("Password", "zaq1@WSX")
                        .formParam("ConfirmPassword", "zaq1@WSX"))
                .pause(5)
                .exec(http("AddItemToBasket")
                        .post("/Basket/AddToBasket")
                        .formParam("id", "19")
                        .formParam("name", ".NET Bot Black Sweatshirt")
                        .formParam("pictureUri", "/images/products/1.png")
                        .formParam("price", "19.50"))
                .pause(2)
                .exec(http("CheckoutBasket")
                        .post("/Basket/Checkout")
                        .formParam("action", "[ Checkout ]"))
                .pause(1)
                .exec(http("CheckOrdersList")
                        .get("/Order/Index")
                .check(substring("Pending")))
//        setUp(scn.inject(atOnceUsers(200))).protocols(httpProtocol)
        setUp(scn.inject(rampUsers(30) over (20 seconds))).protocols(httpProtocol)
}
