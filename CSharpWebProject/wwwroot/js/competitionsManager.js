var CompetitionsManager = /** @class */ (function () {
    function CompetitionsManager() {
    }
    CompetitionsManager.prototype.init = function () {
        this.attachEvents();
    };
    CompetitionsManager.prototype.showAchievement = function () {
        var _this = this;
        $("#achievement .circle").removeClass("rotate");
        // run the animations
        setTimeout(function () {
            $("#achievement").addClass("expand");
            setTimeout(function () {
                $("#achievement").addClass("widen");
                setTimeout(function () {
                    $("#achievement .copy").addClass("show");
                }, 300);
            }, 300);
        }, 300);
        // hide the achievement
        setTimeout(function () {
            _this.hideAchievement();
        }, 4000);
    };
    CompetitionsManager.prototype.hideAchievement = function () {
        setTimeout(function () {
            $("#achievement .copy").removeClass("show");
            setTimeout(function () {
                $("#achievement").removeClass("widen");
                $("#achievement .circle").addClass("rotate");
                setTimeout(function () {
                    $("#achievement").removeClass("expand");
                    $(".refresh").fadeIn(300);
                }, 300);
            }, 300);
        }, 300);
    };
    CompetitionsManager.prototype.attachEvents = function () {
        var self = this;
        $(".joinCompetitionButton").on("click", function () {
            var competitionId = $(this).attr("competition-id");
            $.ajax({
                url: "/Competitions/Join",
                type: "POST",
                data: { id: Number(competitionId) },
                context: document.body
            }).done(function (gotAchievemnt) {
                if (gotAchievemnt) {
                    location.reload();
                    self.showAchievement();
                }
            });
        });
    };
    return CompetitionsManager;
}());
$(document).ready(function () {
    var competitionsManager = new CompetitionsManager();
    competitionsManager.init();
});
