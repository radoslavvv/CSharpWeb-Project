class Timer {
  private isRunning: boolean = false;
  private startTime: number;
  private timerValue: number = 0;
  private spaceIsPressed: boolean = false;
  private timerCanStart: boolean = false;
  private timerFinished: boolean = false;
  private solveTimes = [];

  public init(): any {
    this.attachEvents();
  }

  /**
   * Resets the current state of the timer
   */
  public reset(): void {
    $("#minutes").get(0).innerHTML = "00";
    $("#seconds").get(0).innerHTML = "00";
    $("#milliseconds").get(0).innerHTML = "000";
    this.timerValue = 0;
    this.startTime = null;
    this.isRunning = false;
    this.spaceIsPressed = false;
    this.timerCanStart = false;
    this.timerFinished = false;
  }

  /**
   * Starts the timer
   */
  public start(): void {
    this.startTime = new Date().getTime();
    this.isRunning = true;
    this.increment();
  }

  /**
   * Stops the timer
   */
  public stop(): void {
    this.isRunning = false;
    this.timerFinished = true;

    let minutes: string = $("#minutes").get(0).innerHTML;
    let seconds: string = $("#seconds").get(0).innerHTML;
    let milliseconds: string = $("#milliseconds").get(0).innerHTML;

    let result: string = `${minutes}:${seconds}:${milliseconds}`;

    if (
      window.location.href.toLowerCase().indexOf("competition") !== -1 &&
      this.solveTimes.length === 5
    ) {
      // @ts-ignore
      $("#timesAlert").text("You can't submit more than five times!").alert().fadeIn();
    } else {
      this.solveTimes.push(result);
      if (this.solveTimes.length === 1) {
        $("#timesBox").fadeIn("medium");
      }
      if (window.location.href.toLowerCase().indexOf("competition") !== -1) {
        $(`<li><span class="timeText">${result}</span></li>`)
          .hide()
          .appendTo("#times")
          .fadeIn("slow");
      } else {
        $(
          `<li><span class="timeText">${result}</span>  <a class='delete'>X</a></li>`
        )
          .hide()
          .appendTo("#times")
          .fadeIn("slow");
      }
    }

    if (this.solveTimes.length >= 6) {
      $("#timesContainer").css("overflow-y", "auto");
    }

    let self: this = this;
    $(".delete").on("click", function(e: any): void {
      e.preventDefault();
      e.stopImmediatePropagation();
      e.stopPropagation();
      let time: string = $(this)
        .closest("li")
        .find(".timeText")
        .text();
      let timeIndex: number = self.solveTimes.indexOf(time);
      self.solveTimes.splice(timeIndex, 1);
      console.log(self.solveTimes);
      $(this)
        .closest("li")
        .fadeOut(300, function(): void {
          $(this).remove();
        });
    });
  }

  private showAchievement(): void {
    $("#achievement .circle").removeClass("rotate");
    // run the animations
    setTimeout(() => {
      $("#achievement").addClass("expand");
      setTimeout(() => {
        $("#achievement").addClass("widen");
        setTimeout(() => {
          $("#achievement .copy").addClass("show");
        }, 500);
      }, 500);
    }, 500);
    // hide the achievement
    setTimeout(() => {
      this.hideAchievement();
    }, 1000);
  }

  private hideAchievement(): void {
    setTimeout(() => {
      $("#achievement .copy").removeClass("show");
      setTimeout(() => {
        $("#achievement").removeClass("widen");
        $("#achievement .circle").addClass("rotate");
        setTimeout(() => {
          $("#achievement").removeClass("expand");
          $(".refresh").fadeIn(300);
          window.location.href = "/Home/Index";
        }, 900);
      }, 900);
    }, 1500);
  }
  
/**
 * Save current practice times
 */
  private addTimes(): void {
    if (window.location.href.toLowerCase().indexOf("competition") === -1) {
      $.ajax({
        url: `/Times/AddTimes`,
        type: "POST",
        data: { times: JSON.stringify(this.solveTimes), timeType: "Practice" },
        context: document.body
      }).done(() => {});
      $("#timesAlert")
      .removeClass("alert-danger")
      .addClass("alert-success")
      .text("You successfully saved your times!")
      .alert()
      .fadeIn();
      $("#times").empty();
      this.solveTimes = [];
    }
  }

  /**
   * 
   * Submit current competition times
   */
  private submitTimes(): void {
    let competitionName: string = $("#competitionTitle").text();
    $.ajax({
      url: `/Competitions/AddTimes`,
      type: "POST",
      data: {
        times: JSON.stringify(this.solveTimes),
        timeType: competitionName
      },
      context: document.body
    }).done(e => {
      window.location.href = "/Competitions/";
    });
    $("#times").empty();
    this.solveTimes = [];
  }

  /**
   * Attaches the event listeners to the control buttons
   */

  private attachEvents(): void {
    $("#saveTimesButton").on("click", () => {
      if (
        window.location.href.toLowerCase().indexOf("competition") === -1 &&
        this.solveTimes.length === 0
      ) {
        // @ts-ignore
        $("#timesAlert").text("You must submit at least one time!").alert().fadeIn();
      } else {
        this.addTimes();
      }
    });

    $("#submitTimesButton").on("click", () => {
      if (
        window.location.href.toLowerCase().indexOf("competition") !== -1 &&
        this.solveTimes.length === 0
      ) {
        // @ts-ignore
        $("#timesAlert").text("You must submit at least one time!").alert().fadeIn();
      } else {
        this.submitTimes();
      }
    });

    $(document).on("keydown", e => {
      if (e.keyCode === 32 && !this.spaceIsPressed && !this.timerFinished) {
        this.spaceIsPressed = true;
        $("#timer").css("color", "red");
        setTimeout(() => {
          $("#timer").css("color", "green");
          this.timerCanStart = true;
        }, 200);
      } else if (e.keyCode === 32 && this.isRunning) {
        this.timerCanStart = false;
        this.stop();
      } else if (e.keyCode === 32 && this.timerFinished) {
        this.reset();
      }
    });

    $(document).on("keyup", e => {
      if (e.keyCode === 32 && this.timerCanStart) {
        $("#timer").css("color", "black");
        this.timerCanStart = false;
        this.start();
      } else if (e.keyCode === 32) {
        $("#timer").css("color", "black");
        this.spaceIsPressed = false;
      }
    });
  }

  /**
   * Adds one zero to the beginning of the number
   * @param digit The current digit
   */
  private padMinutesAndSeconds(digit: number): string {
    let result: string = digit.toString();
    if (digit < 10) {
      result = `0${digit}`;
    }

    return result;
  }

  /**
   * Adds two zeroes to the beginning of the number
   * @param digit The current digit
   */
  private padMilliseconds(digit: number): string {
    let result: string = digit.toString();
    if (digit < 10) {
      result = `00${digit}`;
    } else if (digit < 100) {
      result = `0${digit}`;
    }

    return result;
  }

  /**
   * Increments the timer
   */
  private increment(): void {
    if (this.isRunning) {
      let currentTime: number = new Date().getTime();

      let diff: number = currentTime - this.startTime;

      this.timerValue += diff;

      let currentTimeValue: Date = new Date(this.timerValue);
      let minutes: string = this.padMinutesAndSeconds(
        currentTimeValue.getMinutes()
      );
      let seconds: string = this.padMinutesAndSeconds(
        currentTimeValue.getSeconds()
      );
      let milliseconds: string = this.padMilliseconds(
        currentTimeValue.getMilliseconds()
      );

      $("#minutes").get(0).innerHTML = minutes;
      $("#seconds").get(0).innerHTML = seconds;
      $("#milliseconds").get(0).innerHTML = milliseconds;
      this.startTime = currentTime;

      setTimeout(() => {
        this.increment();
      }, 10);
    }
  }
}

$(document).ready(() => {
  let timer: Timer = new Timer();
  timer.init();
});
