var Timer = /** @class */ (function () {
    function Timer() {
        this.isRunning = false;
        this.timerValue = 0;
        this.spaceIsPressed = false;
        this.timerCanStart = false;
        this.timerFinished = false;
        this.solveTimes = [];
        // public start(): void {
        //     this.isRunning = true;
        //     this.increment();
        // }
        // public stop(): void{
        //     this.isRunning = false;
        // }
        // private increment(): void {
        //     if (this.isRunning) {
        //         setTimeout(() => {
        //             this.time++;
        //             let mins: any = Math.floor(this.time / 10 / 60);
        //             let secs: any = Math.floor(this.time / 10);
        //             let tenths: any = (this.time % 100);
        //             // let hundreds: any = this.time;
        //             tenths = tenths   + "0" ;
        //             if(secs >= 60){
        //                 mins++;
        //                 secs = 0;
        //             }
        //             if (mins < 10) {
        //                 mins = "0" + mins;
        //             }
        //             if (secs < 10) {
        //                 secs = "0" + secs;
        //             }
        //             // console.log();
        //             $("#timer").get(0).innerHTML = `${mins} ${secs} ${tenths}`;
        //             console.log(this.time);
        //             this.increment();
        //         }, 100);
        //     }
        // }
        // public reset(): void {
        //     this.isRunning = false;
        //     this.time = 0;
        // }
    }
    Timer.prototype.init = function () {
        this.attachEvents();
    };
    /**
     * Resets the current state of the timer
     */
    Timer.prototype.reset = function () {
        $("#minutes").get(0).innerHTML = "00";
        $("#seconds").get(0).innerHTML = "00";
        $("#milliseconds").get(0).innerHTML = "000";
        this.timerValue = 0;
        this.startTime = null;
        this.isRunning = false;
        this.spaceIsPressed = false;
        this.timerCanStart = false;
        this.timerFinished = false;
    };
    /**
     * Starts the timer
     */
    Timer.prototype.start = function () {
        this.startTime = new Date().getTime();
        this.isRunning = true;
        this.increment();
    };
    /**
     * Stops the timer
     */
    Timer.prototype.stop = function () {
        this.isRunning = false;
        this.timerFinished = true;
        var minutes = $("#minutes").get(0).innerHTML;
        var seconds = $("#seconds").get(0).innerHTML;
        var milliseconds = $("#milliseconds").get(0).innerHTML;
        var result = minutes + ":" + seconds + ":" + milliseconds;
        if (window.location.href.toLowerCase().indexOf("competition") !== -1 &&
            this.solveTimes.length === 5) {
            // @ts-ignore
            $("#timesAlert").text("You can't submit more than five times!").alert().fadeIn();
        }
        else {
            this.solveTimes.push(result);
            if (this.solveTimes.length === 1) {
                $("#timesBox").fadeIn("medium");
            }
            if (window.location.href.toLowerCase().indexOf("competition") !== -1) {
                $("<li><span class=\"timeText\">" + result + "</span></li>")
                    .hide()
                    .appendTo("#times")
                    .fadeIn("slow");
            }
            else {
                $("<li><span class=\"timeText\">" + result + "</span>  <a class='delete'>X</a></li>")
                    .hide()
                    .appendTo("#times")
                    .fadeIn("slow");
            }
        }
        if (this.solveTimes.length >= 6) {
            $("#timesContainer").css("overflow-y", "auto");
        }
        // $(`<li><span class="timeText">${result}</span>  <a class='delete'>X</a></li>`).hide().appendTo("#times").fadeIn("slow");
        var self = this;
        $(".delete").on("click", function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();
            e.stopPropagation();
            var time = $(this)
                .closest("li")
                .find(".timeText")
                .text();
            var timeIndex = self.solveTimes.indexOf(time);
            self.solveTimes.splice(timeIndex, 1);
            console.log(self.solveTimes);
            $(this)
                .closest("li")
                .fadeOut(300, function () {
                $(this).remove();
            });
        });
    };
    Timer.prototype.showAchievement = function () {
        var _this = this;
        $("#achievement .circle").removeClass("rotate");
        // run the animations
        setTimeout(function () {
            $("#achievement").addClass("expand");
            setTimeout(function () {
                $("#achievement").addClass("widen");
                setTimeout(function () {
                    $("#achievement .copy").addClass("show");
                }, 500);
            }, 500);
        }, 500);
        // hide the achievement
        setTimeout(function () {
            _this.hideAchievement();
        }, 1000);
    };
    Timer.prototype.hideAchievement = function () {
        setTimeout(function () {
            $("#achievement .copy").removeClass("show");
            setTimeout(function () {
                $("#achievement").removeClass("widen");
                $("#achievement .circle").addClass("rotate");
                setTimeout(function () {
                    $("#achievement").removeClass("expand");
                    $(".refresh").fadeIn(300);
                    window.location.href = "/Home/Index";
                }, 900);
            }, 900);
        }, 1500);
    };
    Timer.prototype.addTimes = function () {
        if (window.location.href.toLowerCase().indexOf("competition") === -1) {
            $.ajax({
                url: "/Times/AddTimes",
                type: "POST",
                data: { times: JSON.stringify(this.solveTimes), timeType: "Practice" },
                context: document.body
            }).done(function () { });
            $("#timesAlert")
                .removeClass("alert-danger")
                .addClass("alert-success")
                .text("You successfully saved your times!")
                .alert()
                .fadeIn();
            $("#times").empty();
            this.solveTimes = [];
        }
    };
    Timer.prototype.submitTimes = function () {
        var competitionName = $("#competitionTitle").text();
        $.ajax({
            url: "/Competitions/AddTimes",
            type: "POST",
            data: {
                times: JSON.stringify(this.solveTimes),
                timeType: competitionName
            },
            context: document.body
        }).done(function (e) {
            window.location.href = "/Competitions/";
        });
        $("#times").empty();
        this.solveTimes = [];
    };
    /**
     * Attaches the event listeners to the control buttons
     */
    Timer.prototype.attachEvents = function () {
        var _this = this;
        $("#saveTimesButton").on("click", function () {
            if (window.location.href.toLowerCase().indexOf("competition") === -1 &&
                _this.solveTimes.length === 0) {
                // @ts-ignore
                $("#timesAlert").text("You must submit at least one time!").alert().fadeIn();
            }
            else {
                _this.addTimes();
            }
        });
        $("#submitTimesButton").on("click", function () {
            if (window.location.href.toLowerCase().indexOf("competition") !== -1 &&
                _this.solveTimes.length === 0) {
                // @ts-ignore
                $("#timesAlert").text("You must submit at least one time!").alert().fadeIn();
            }
            else {
                _this.submitTimes();
            }
        });
        $(document).on("keydown", function (e) {
            if (e.keyCode === 32 && !_this.spaceIsPressed && !_this.timerFinished) {
                _this.spaceIsPressed = true;
                $("#timer").css("color", "red");
                setTimeout(function () {
                    $("#timer").css("color", "green");
                    _this.timerCanStart = true;
                }, 200);
            }
            else if (e.keyCode === 32 && _this.isRunning) {
                _this.timerCanStart = false;
                _this.stop();
            }
            else if (e.keyCode === 32 && _this.timerFinished) {
                _this.reset();
            }
        });
        $(document).on("keyup", function (e) {
            if (e.keyCode === 32 && _this.timerCanStart) {
                $("#timer").css("color", "black");
                _this.timerCanStart = false;
                _this.start();
            }
            else if (e.keyCode === 32) {
                $("#timer").css("color", "black");
                _this.spaceIsPressed = false;
            }
        });
    };
    /**
     * Adds one zero to the beginning of the number
     * @param digit The current digit
     */
    Timer.prototype.padMinutesAndSeconds = function (digit) {
        var result = digit.toString();
        if (digit < 10) {
            result = "0" + digit;
        }
        return result;
    };
    /**
     * Adds two zeroes to the beginning of the number
     * @param digit The current digit
     */
    Timer.prototype.padMilliseconds = function (digit) {
        var result = digit.toString();
        if (digit < 10) {
            result = "00" + digit;
        }
        else if (digit < 100) {
            result = "0" + digit;
        }
        return result;
    };
    /**
     * Increments the timer
     */
    Timer.prototype.increment = function () {
        var _this = this;
        if (this.isRunning) {
            var currentTime = new Date().getTime();
            var diff = currentTime - this.startTime;
            this.timerValue += diff;
            var currentTimeValue = new Date(this.timerValue);
            var minutes = this.padMinutesAndSeconds(currentTimeValue.getMinutes());
            var seconds = this.padMinutesAndSeconds(currentTimeValue.getSeconds());
            var milliseconds = this.padMilliseconds(currentTimeValue.getMilliseconds());
            $("#minutes").get(0).innerHTML = minutes;
            $("#seconds").get(0).innerHTML = seconds;
            $("#milliseconds").get(0).innerHTML = milliseconds;
            this.startTime = currentTime;
            setTimeout(function () {
                _this.increment();
            }, 10);
        }
    };
    return Timer;
}());
$(document).ready(function () {
    var timer = new Timer();
    timer.init();
});
