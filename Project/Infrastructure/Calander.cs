using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Infrastructure
{


    public class Calander
    {
        public Calander()
        {

        }

        private string month_name;
        private string week_name;
        private string ayyam_name;
        private string ayyam_name2;
        private string ayyam_name3;

        DateTime mydate;
        PersianCalendar mycal = new PersianCalendar();
        HijriCalendar hijrical = new HijriCalendar();

        public Calander(DateTime dt)
        {
            mydate = dt;
        }

        private string hijricalendar()
        {
            return hijrical.GetDayOfMonth(mydate).ToString() + " / " + hijrical.GetMonth(mydate).ToString() + " / " + hijrical.GetYear(mydate).ToString();
        }

        private string miladicalendar()
        {
            return mydate.Day.ToString() + " / " + mydate.Month.ToString() + " / " + mydate.Year.ToString();
        }

        private string persianmonth()
        {
            switch (mycal.GetMonth(mydate))
            {
                case 1: month_name = "فروردین"; break;
                case 2: month_name = "اردیبهشت"; break;
                case 3: month_name = "خرداد"; break;
                case 4: month_name = "تیر"; break;
                case 5: month_name = "مرداد"; break;
                case 6: month_name = "شهریور"; break;
                case 7: month_name = "مهر"; break;
                case 8: month_name = "آبان"; break;
                case 9: month_name = "آذر"; break;
                case 10: month_name = "دی"; break;
                case 11: month_name = "بهمن"; break;
                case 12: month_name = "اسفند"; break;
            }
            return month_name;
        }

        private string persianweek()
        {
            switch (mycal.GetDayOfWeek(mydate).ToString().ToLower())
            {
                case "saturday": week_name = "شنبه"; break;
                case "sunday": week_name = "یکشنبه"; break;
                case "monday": week_name = "دوشنبه"; break;
                case "tuesday": week_name = "سه شنبه"; break;
                case "wednesday": week_name = "چهارشنبه"; break;
                case "thursday": week_name = "پنج شنبه"; break;
                case "friday": week_name = "جمعه"; break;
            }
            return week_name;
        }

        private string ayyam()
        {
            switch (mycal.GetMonth(mydate))
            {
                case 1:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name = "سال نو بر شما مبارک باد"; break;
                        case 2: ayyam_name = "هجوم ماموران ستم شاهی به مدرسه ی فیضیه ی قم"; break;
                        case 12: ayyam_name = "روز جمهوری اسلامی ایران -- تعطیل"; break;
                        case 13: ayyam_name = "روز طبیعت -- تعطیل"; break;
                        case 18: ayyam_name = "روز سلامتی - روز جهانی بهداشت"; break;
                        case 19: ayyam_name = "شهادت آیت اله سید محمد باقر صدر و خواهر ایشان بنت الهدی توسط رژیم بعث عراق"; break;
                        case 20: ayyam_name = "روز ملی فناوری هسته ای"; break;
                        case 21: ayyam_name = "شهادت امیر سپهبد علی صیاد شیرازی"; break;
                        case 25: ayyam_name = "روز بزرگداشت عطار نیشابوری"; break;
                        case 29: ayyam_name = "روز ارتش جمهوری اسلامی ایران"; break;
                    }
                    break;
                case 2:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name = "روز بزرگداشت سعدی"; break;
                        case 2: ayyam_name = "تاسیس سپاه پاسداران انتقلاب اسلامی - سالروز اعلام انقلاب فرهنگی - روز زمین پاک"; break;
                        case 3: ayyam_name = "روز بزرگداشت شیخ بهایی - روز ملی کار آفرینی"; break;
                        case 5: ayyam_name = "شکست حمله نظامی آمریکا به ایران در طبس"; break;
                        case 9: ayyam_name = "روز شوراها"; break;
                        case 10: ayyam_name = " روز ملی خلیج فارس - آغاز عملیات بیت المقدس"; break;
                        case 12: ayyam_name = "شهادت استاد مرتضی مطهری - روز معلم - روز جهانی کار و کارگر"; break;
                        case 15: ayyam_name = "روز بزرگداشت شیخ صدوق"; break;
                        case 17: ayyam_name = "روز اسناد ملی"; break;
                        case 19: ayyam_name = "روز جهانی صلیب سرخ و حلال احمر"; break;
                        case 24: ayyam_name = "لغو امتیاز تنباکو به فتوای آیت الله میرزا حسن شیرازی"; break;
                        case 25: ayyam_name = "روز بزرگداشت فردوسی"; break;
                        case 27: ayyam_name = "روز جهانی ارتباطات و روابط عمومی"; break;
                        case 28: ayyam_name = "روز بزرگداشت حکیم عمر خیام"; break;
                        case 29: ayyam_name = "روز جهانی موزه و میراث فرهنگی"; break;
                    }
                    break;
                case 3:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name = "روز بهره وری و بهینه سازی مصرف - روز بزرگداشت ملا صدرا"; break;
                        case 3: ayyam_name = "فتح خرم شهر در عملیات بیت امقدس و روز مقاومت ، ایثار و پیروزی"; break;
                        case 14: ayyam_name = "رحلت حضرت امام خمینی -- تعطیل"; break;
                        case 15: ayyam_name = "قیام خونین 15 خرداد -- تعطیل"; break;
                        case 16: ayyam_name = "روز جهانی محیط زیست"; break;
                        case 20: ayyam_name = "شهادت آیت الله سعیدی به دست ماموران ستم شاهی پهلوی"; break;
                        case 24: ayyam_name = "روز جهانی صنایع دستی"; break;
                        case 25: ayyam_name = "روز گل و گیاه"; break;
                        case 26: ayyam_name = "شهادت سربازان دلیر اسلام،بخارایی،امانی،صفار هرندی و نیک نژاد"; break;
                        case 27: ayyam_name = "روز جهاد کشاورزی -- تشکیل جهاد سازندگی به فرمان امام"; break;
                        case 28: ayyam_name = "روز جهانی بیابان زدایی"; break;
                        case 29: ayyam_name = "درگذشت دکتر علی شریعتی"; break;
                        case 30: ayyam_name = "انفجار در حرم حضرت امام رضا به دست منافقین کور دل"; break;
                        case 31: ayyam_name = "شهادت دکتر مصطفی چمران"; break;
                    }
                    break;
                case 4:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name = "روز تبلیغ و اطلاع رسانی دینی - روز اصناف"; break;
                        case 6: ayyam_name = "روز جهانی مبارزه با مواد مخدر"; break;
                        case 7: ayyam_name = "شهادت آیت الله دکتر بهشتی و 72 تن از یاران امام - روز قوه قضاییه"; break;
                        case 8: ayyam_name = "روز مبارزه با صلاح های میکروبی و شیمیایی"; break;
                        case 10: ayyam_name = "روز صنعت و معدن"; break;
                        case 11: ayyam_name = "شهادت آیت الله صدوقی چهارمین شهید محراب به دست به دست منافقین"; break;
                        case 12: ayyam_name = "سقوط هواپیمای مسافر بری جمهوری اسلامی ایران توسط آمریکا"; break;
                        case 14: ayyam_name = "روز قلم"; break;
                        case 16: ayyam_name = "روز مالیات"; break;
                        case 25: ayyam_name = "روز بهزیستی و تامین اجتماعی"; break;
                        case 27: ayyam_name = "اعلام پذیرش قطعنامه شورای امنیت از سوی ایران"; break;

                    }
                    break;
                case 5:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 5: ayyam_name = "سالروز عملیات افتخار آفرین مرصاد"; break;
                        case 6: ayyam_name = "روز ترویج آموزش های فنی و حرفه ای"; break;
                        case 8: ayyam_name = "روز بزرگداشت شیخ شهاب الدین سهروردی شیخ اشراق"; break;
                        case 9: ayyam_name = "روز اهدای خون"; break;
                        case 14: ayyam_name = "صدور فرمان مشروطیت"; break;
                        case 16: ayyam_name = "تشکیل جهاد دانشگاهی "; break;
                        case 17: ayyam_name = "روز خبرنگار"; break;
                        case 26: ayyam_name = "آغاز بازگشت آزادگان به میهن اسلامی"; break;
                        case 28: ayyam_name = "کودتای آمریکا برای بازگرداندن شاه"; break;
                        case 30: ayyam_name = "روز بزرگداشت علامه مجلسی"; break;
                        case 31: ayyam_name = "روز جهانی مسجد"; break;
                    }
                    break;
                case 6:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name = "روز پزشک - روز بزرگداشت ابوعلی سینا"; break;
                        case 2: ayyam_name = "آغاز هفته دولت"; break;
                        case 4: ayyam_name = "روز کارمند"; break;
                        case 5: ayyam_name = "روز دارو سازی - روز بزرگداشت محمد بن زکریای رازی"; break;
                        case 8: ayyam_name = "روز مبارزه با تروریسم - انفجار دفتر نخست وزیری"; break;
                        case 10: ayyam_name = "روز بانکداری اسلامی - سالروز تصویب قانون عملیات بانکی بدون ربا"; break;
                        case 11: ayyam_name = "روز صنعت چاپ"; break;
                        case 13: ayyam_name = "روز تعاون - روز بزرگداش ابو ریحان بیرونی"; break;
                        case 14: ayyam_name = "شهادت آیت الله قدوسی و سرتیپ وحید دستجردی"; break;
                        case 17: ayyam_name = "قیام 17 شهریور و کشتار جمعی از مردم به دست ماموران پهلوی"; break;
                        case 19: ayyam_name = "وفات آیت الله سید محمد طالقانی اولین امام جمعه تهران"; break;
                        case 20: ayyam_name = "شهادت دوین شهید محراب آیت الله مدنی به دست منافقین"; break;
                        case 21: ayyam_name = "روز سینما"; break;
                        case 27: ayyam_name = "روز شعر و ادب فارسی - وز بزرگداشت استاد سید محمد حسین شهریار"; break;
                        case 31: ayyam_name = "آغاز جنگ تحمیلی - آغاز هفته ی دفاع مقدس"; break;
                    }
                    break;
                case 7:
                    switch (mycal.GetDayOfMonth(mydate))
                    {


                        case 5: ayyam_name = "شکست حصر آبادان در عملیات ثامن الائمه"; break;
                        case 6: ayyam_name = "روز جهانی جهانگردی"; break;
                        case 7: ayyam_name = "روز آتشنشانی و ایمنی - شهادت سرداران اسلام"; break;
                        case 8: ayyam_name = "روز بزرگداشت مولوی"; break;
                        case 9: ayyam_name = "روز جهانی ناشنوایان و روز همبستگی کودکان و نوجوانان فلسطینی"; break;
                        case 13: ayyam_name = "هجرت حضرت امام خمینی ره از عراق به پاریس - روز نیروی انتظامی"; break;
                        case 14: ayyam_name = "روز دامپزشکی"; break;
                        case 17: ayyam_name = "روز جهانی کودک "; break;
                        case 20: ayyam_name = "روز بزگداشت حافظ - روز اسکان معلولان و سالمندان - روز ملی کاهش بلایای طبیعی"; break;
                        case 23: ayyam_name = "شهادت پنجمین شهید معراب آیت الله اشرفی اصفهانی - روز جهانی استاندارد"; break;
                        case 24: ayyam_name = "روز پیوند اولیا و مربیان - روز جهانی نابینایان عصای سفید"; break;
                        case 26: ayyam_name = "روز تربیت بدنی و ورزش"; break;
                        case 29: ayyam_name = "روز صادرات"; break;
                    }
                    break;
                case 8:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name = "روز آمار  برنامه ریزی"; break;
                        case 4: ayyam_name = "اعتراض افشاگری حضرت امام خمینی ره علیه پذیرش کاپیتولاسیون"; break;
                        case 8: ayyam_name = "شهادت محمد حسین فهمیده - روز نوجوان - روز بسیج دانش آموزی"; break;
                        case 10: ayyam_name = "شهادت آیت الله قاضی طباطبایی اولین شهید محراب"; break;
                        case 13: ayyam_name = "روز ملی مبارزه با استکبار جهانی - روز دانش آموز - تسخیر لانه جاسوسی آمریکا به دست دانشجویان پیرو خط امام"; break;
                        case 14: ayyam_name = "روز فرهنگ عمومی"; break;
                        case 18: ayyam_name = "روز ملی کیفیت"; break;
                        case 24: ayyam_name = "روز کتابخوانی - روز بزرگداشت علامه سید محمد حسین طباطبایی"; break;
                    }
                    break;
                case 9:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 5: ayyam_name = "روز بسیج مستضعفان - تشکیل بسیج مستضعفین به فرمان حضرت امام خمینی ره"; break;
                        case 7: ayyam_name = "روز نیروی دریایی"; break;
                        case 9: ayyam_name = "روز بزرگداشت شیخ مفید"; break;
                        case 10: ayyam_name = "شهادت آیت سید حسن مدرس و روز مجلس"; break;
                        case 12: ayyam_name = "تصویب قانون اساسی جمهوری اسلامی ایران"; break;
                        case 13: ayyam_name = "روز جهانی معلولان و روز بیمه"; break;
                        case 15: ayyam_name = "شهادت مظلومانه زائران خانه ی خدا به دستور آمریکا"; break;
                        case 16: ayyam_name = "روز داشجو"; break;
                        case 18: ayyam_name = "معرفی عراق بعنوان مسئول و آغاز جنگ از سوی سازمان ملل"; break;
                        case 19: ayyam_name = "تشکیل شورای انقلاب فرهنگی به فرمان حضرت امام خمینی ره "; break;
                        case 20: ayyam_name = "شهادت آیت الله دست غیب سومین شهید محراب به دست منافقین"; break;
                        case 25: ayyam_name = "روز پژوهش"; break;
                        case 26: ayyam_name = "روز حمل ونقل"; break;
                        case 27: ayyam_name = "شهادت آیت الله دکتر محمد مفتح - روز وحدت حوزه و دانشگاه"; break;
                    }
                    break;
                case 10:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 5: ayyam_name = "روز ملی ایمنی در برابر زلزله"; break;
                        case 7: ayyam_name = "سالروز تشکیل نهضت سوادآموزی به فرمان حضرت امام خمینی ره - شهادت آیت الله حسین غفاری به دست پهلوی"; break;
                        case 19: ayyam_name = "قیام خونین مردم قم - روز تجلیل از اسرا و مفقودان"; break;
                        case 20: ayyam_name = "شهادت میرزا تقی خان امیر کبیر"; break;
                        case 22: ayyam_name = "تشکیل شورای انقلاب به فرمان حضرت امام خمینی ره"; break;
                        case 26: ayyam_name = "فرار شاه معدوم"; break;
                        case 27: ayyam_name = "شهادت نواب صفوی ، طهماسبی ، برادران واحدی و ذوالقدر از فداییان اسلام"; break;
                    }
                    break;
                case 11:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 6: ayyam_name = "سالروز حماسه مردم آمل"; break;
                        case 12: ayyam_name = "بازگشت حضرت امام خمینی ره به ایران و آغاز دهه ی مبارک فجر"; break;
                        case 14: ayyam_name = " پرتاب موفقيت آميز ماهواره اميد به فضا و بازتاب آن در رسانه هاي جهان "; break;
                        case 19: ayyam_name = "روز نیروی هوایی"; break;
                        case 22: ayyam_name = "پیروزی انقلاب و سقوط شاهنشاهی -- تعطیل"; break;
                        case 29: ayyam_name = "قیام مردم تبریز چهلمین روز شهادت شهدای قم"; break;
                    }
                    break;
                case 12:
                    switch (mycal.GetDayOfMonth(mydate))
                    {
                        case 5: ayyam_name = "روز بزرگداشت خواجه نصیرالدین طوسی - روز مهندسی - روز وقف"; break;
                        case 8: ayyam_name = "روز امور تربیتی و تربیت اسلامی"; break;
                        case 9: ayyam_name = "روز ملی حمایت از حقوق مصرف کنندگاه"; break;
                        case 14: ayyam_name = "روز احسان و نیکوکاری"; break;
                        case 15: ayyam_name = "روز درختکاری"; break;
                        case 22: ayyam_name = "روز بزرگداشت شهدا"; break;
                        case 25: ayyam_name = "روز اخلاق و مهرورزی -  بمباران شیمیایی حلبچه توسط عراق"; break;
                        case 29: ayyam_name = "روز ملی شدن صنعت نفت ایران -- تعطیل"; break;
                    }
                    break;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            switch (hijrical.GetMonth(mydate))
            {
                case 1:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name2 = "آغاز سال جدید قمری"; break;
                        case 9: ayyam_name2 = "تاسوعای حسینی -- تعطیل"; break;
                        case 10: ayyam_name2 = "عاشورای حسینی -- تعطیل"; break;
                        case 12: ayyam_name2 = " شهادت حضرت زین العابدین ع"; break;
                        case 18: ayyam_name2 = "تغییر قبله مسلمین از بیت المقدس به مکه"; break;
                        case 25: ayyam_name2 = "شهادت امام زین العابدین علیه السلام به روایتی"; break;
                    }
                    break;
                case 2:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 3: ayyam_name2 = "ولادت حضرت امام محمد باقر ع"; break;
                        case 7: ayyam_name2 = "ولادت حضرت امام موسی کاظم ع"; break;
                        case 20: ayyam_name2 = "اربعین حسینی -- تعطیل"; break;
                        case 28: ayyam_name2 = "رحلت حضرت رسول اکرم ص - شهادت حضرت امام حسن مجتبی ع -- تعطیل"; break;
                        case 30: ayyam_name2 = "شهادت حضرت امام رضا ع - تعطیل"; break;
                    }
                    break;
                case 3:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name2 = "هجرت حضرت رسول ص از مکه به مدینه - مبداگاه شماری هجری قمری"; break;
                        case 8: ayyam_name2 = "شهادت حضرت امام حسن عسگری ع"; break;
                        case 12: ayyam_name2 = "میلاد حضرت رسول اکرم به روایت اهل سنت - آغاز هفته وحدت"; break;
                        case 17: ayyam_name2 = "میلاد حضرت رسول اکرم و روز اخلاق و مهرورزی -- میلاد امام جعفر صادق -- تعطیل"; break;
                    }
                    break;
                case 4:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 8: ayyam_name2 = "ولادت امام حسن عسکری علیه السلام"; break;
                        case 10: ayyam_name2 = "(وفات حضرت معصومه (س"; break;
                    }
                    break;
                case 5:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 5: ayyam_name2 = "ولادت حضرت زینب س - روز پرستار و بهورز"; break;
                    }
                    break;
                case 6:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 3: ayyam_name2 = "شهادت حضرت فاطمه زهرا س -- تعطیل"; break;
                        case 30: ayyam_name2 = "ولادت حضرت فاطمه زهرا - ولادت حضرت امام خمینی"; break;
                    }
                    break;
                case 7:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name2 = "ولادت حضرت امام محمد باقر"; break;
                        case 3: ayyam_name2 = "شهادت حضرت امام علی النقی الهادی "; break;
                        case 10: ayyam_name2 = "ولادت حضرت امام محمد تقی ع جواد الائمه"; break;
                        case 13: ayyam_name2 = "ولادت حضرت امام علی  علیه السلام - آغاز ایام اعتکاف -- تعطیل"; break;
                        case 15: ayyam_name2 = "وفات حضرت زینب"; break;
                        case 25: ayyam_name2 = "شهادت حضرت امام موسی کاظم ع"; break;
                        case 27: ayyam_name2 = "مبعث رسول اکرم ص -- تعطیل"; break;
                    }
                    break;
                case 8:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 3: ayyam_name2 = "ولادت حضرت امام حسین ع و روز پاسدار"; break;
                        case 4: ayyam_name2 = "ولادت حضرت ابوالفضل العباس و روز جانباز"; break;
                        case 5: ayyam_name2 = "ولادت حضرت امام زین العابدین ع"; break;
                        case 11: ayyam_name2 = "ولادت حضرت علی اکبر ع و روز جوان"; break;
                        case 15: ayyam_name2 = "ولادت حضرت قائم عج روز جهانی مستضعفان -- تعطیل"; break;
                    }
                    break;
                case 9:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 10: ayyam_name2 = "وفات حضرت خدیجه س"; break;
                        case 15: ayyam_name2 = "ولادت حضرت امام حسن مجتبی علیه السلام و روز اکرام"; break;
                        case 18: ayyam_name2 = "شب قدر"; break;
                        case 19: ayyam_name2 = " ضربت خوردن حضرت علی ع روز گفت و گوی تمدنها"; break;
                        case 20: ayyam_name2 = "شب قدر"; break;
                        case 21: ayyam_name2 = "شهادت حضرت علی علیه السلام -- تعطیل"; break;
                        case 22: ayyam_name2 = "شب قدر"; break;
                    }
                    break;
                case 10:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name2 = "عید سعید فطر -- تعطیل"; break;
                        case 3: ayyam_name2 = "سالروز شهادت حضرت سلطان علی بن امام محمد باقر"; break;
                        case 25: ayyam_name2 = "شهادت امام جعفر صادق ع -- تعطیل"; break;
                    }
                    break;
                case 11:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name2 = "ولادت حضرت معصومه س"; break;
                        case 11: ayyam_name2 = "ولادت حضرت امام رضا ع"; break;
                        case 29: ayyam_name2 = "شهادت امام محمد تقی ع جواد الائمه"; break;
                    }
                    break;
                case 12:
                    switch (hijrical.GetDayOfMonth(mydate))
                    {
                        case 1: ayyam_name2 = "سالروز ازدواج حضرت علی ع و حضرت فاطمه س"; break;
                        case 7: ayyam_name2 = "شهادت امام محمد باقر ع"; break;
                        case 9: ayyam_name2 = "روز عرفه - روز نیایش"; break;
                        case 10: ayyam_name2 = "عید سعید قربان -- تعطیل "; break;
                        case 15: ayyam_name2 = "ولادت حضرت امام علی النقی الهادی ع"; break;
                        case 18: ayyam_name2 = "روز غدیر خم "; break;
                        case 24: ayyam_name2 = "روز مباهله پیامبر اسلام ص"; break;
                        case 25: ayyam_name2 = " روز خانواده وتکریم بازنشستگان "; break;
                    }
                    break;

            }
            if (mydate.Month == 1 && mydate.Day == 1)
                ayyam_name3 = ayyam_name + " - " + ayyam_name2 + " - " + "آغاز سال جدید میلادی";
            else if (mydate.Month == 12 && mydate.Day == 25)
                ayyam_name3 = ayyam_name + " - " + ayyam_name2 + " - " + "میلاد حضرت عیسی مسیح علیه السلام";
            //////////////////////////////////////////////////////////////////////////////////////////////////
            return ayyam_name3 = ayyam_name2 + " - " + ayyam_name; ;
        }

        public DateTime PreviousMonthFirstDay(DateTime currentDate)
        {
            DateTime d = PreviousMonthLastDay(currentDate);

            return new DateTime(d.Year, d.Month, 1);
        }

        public DateTime PreviousMonthLastDay(DateTime currentDate)
        {
            return new DateTime(currentDate.Year, currentDate.Month, 1).AddDays(-1);
        }

        /// <summary>
        /// ساعت سیستم بصورت اصلاح شده
        /// </summary>
        /// <returns></returns>
        public string time()
        {
            return DateTime.Now.Second.ToString() + " : " + DateTime.Now.Minute.ToString() + " : " + DateTime.Now.Hour.ToString();
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// کل تقویم شمسی در یک رشته
        /// </summary>
        /// <returns></returns>
        public string taghvim()
        {
            return ayyam();
        }

        /// <summary>
        /// تاریخ شمسی همراه با مصادف ها
        /// </summary>
        /// <returns></returns>
        public string CompletePrsDate()
        {
            string str;
            str = persianweek() + "   " + mycal.GetDayOfMonth(mydate).ToString() + "   " + persianmonth() + "   سال   " + mycal.GetYear(mydate).ToString();
            str += "   مصادف با   " + miladicalendar();
            return str;
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// تاریخ شمسی ساده برای دخیره در دیتابیس
        /// </summary>
        /// <returns></returns>
        public string Persion()
        {
            string strYear = mycal.GetYear(mydate).ToString();
            string strMonth = (mycal.GetMonth(mydate).ToString().Length == 1) ? ("0" + mycal.GetMonth(mydate)) : (mycal.GetMonth(mydate).ToString());
            string strDay = (mycal.GetDayOfMonth(mydate).ToString().Length == 1) ? ("0" + mycal.GetDayOfMonth(mydate)) : (mycal.GetDayOfMonth(mydate).ToString());
            return strYear + "/" + strMonth + "/" + strDay;
        }

        public string GetTimeNow()
        {
            return DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        }

        public string bebDate()
        {
            string strYear = mycal.GetYear(mydate).ToString();
            string strMonth = (mycal.GetMonth(mydate).ToString().Length == 1) ? ("0" + mycal.GetMonth(mydate)) : (mycal.GetMonth(mydate).ToString());
            string strDay = (mycal.GetDayOfMonth(mydate).ToString().Length == 1) ? ("0" + mycal.GetDayOfMonth(mydate)) : (mycal.GetDayOfMonth(mydate).ToString());
            return strYear.Substring(2, 2) + strMonth + strDay;
        }

        public System.DateTime miladyDate(string splitday)
        {
            string persianDate = splitday;
            string[] split1 = persianDate.Split(' ');
            string[] split = split1[0].Split('/');
            int day = Convert.ToInt32(split[0]);
            int month = Convert.ToInt32(split[1]);
            int year = Convert.ToInt32(split[2]);
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            DateTime dt1 = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            return dt1;
        }

        public string Time()
        {
            string TimeNow = "";
            string Minute, Second, Hour;
            Hour = DateTime.Now.TimeOfDay.Hours.ToString();
            Minute = DateTime.Now.TimeOfDay.Minutes.ToString();
            Second = DateTime.Now.TimeOfDay.Seconds.ToString();
            TimeNow = Hour + ":" + Minute + ":" + Second;
            return TimeNow;
        }

        public string YearsNow()
        {
            string Year;
            string Month;
            string Day;
            PersianCalendar PrsClnd = new PersianCalendar();
            DateTime DT = DateTime.Now;

            Year = PrsClnd.GetYear(DT).ToString();
            Month = PrsClnd.GetMonth(DT).ToString();
            Day = PrsClnd.GetDayOfMonth(DT).ToString();
            return Year + "/" + Month + "/" + Day;
        }

        public int Year(System.DateTime dateTime)
        {
            string Year;
            string Month;
            string Day;
            PersianCalendar PrsClnd = new PersianCalendar();
            DateTime DT = dateTime;

            Year = PrsClnd.GetYear(DT).ToString();
            Month = PrsClnd.GetMonth(DT).ToString();
            Day = PrsClnd.GetDayOfMonth(DT).ToString();
            return Convert.ToInt32(Year);
        }

        public int Month(System.DateTime dateTime)
        {
            string Year;
            string Month;
            string Day;
            PersianCalendar PrsClnd = new PersianCalendar();
            DateTime DT = dateTime;

            Year = PrsClnd.GetYear(DT).ToString();
            Month = PrsClnd.GetMonth(DT).ToString();
            Day = PrsClnd.GetDayOfMonth(DT).ToString();
            return Convert.ToInt32(Month);
        }

        public int GetNumberDayInMonth()
        {
            int Res;
            try
            {
                PersianCalendar DateFme = new PersianCalendar();
                Res = DateFme.GetDayOfMonth(DateTime.Now);
            }
            catch
            {
                Res = 0;
            }
            return Res;
        }

        public string GetNameDayInMonth(System.DateTime dateTime)
        {

            string Resme = "";
            string Res;
            PersianCalendar DateFme = new PersianCalendar();
            Res = DateFme.GetDayOfWeek(dateTime).ToString();
            switch (Res)
            {
                case "Saturday":
                    {
                        Resme = "شنبه";
                        break;
                    }
                case "Sunday":
                    {
                        Resme = "یکشبه";
                        break;
                    }
                case "Monday":
                    {
                        Resme = "دو شنبه";
                        break;
                    }
                case "Tuesday":
                    {
                        Resme = "سه شبه";
                        break;
                    }
                case "Wednesday":
                    {
                        Resme = "چهار شنبه";
                        break;
                    }
                case "Thursday":
                    {
                        Resme = "پنج شنبه";
                        break;
                    }
                case "Friday":
                    {
                        Resme = "جمعه";
                        break;
                    }
            }
            return Resme;
        }

        public string GetNameMonth(System.DateTime oDateTime)
        {

            string Resme = "";
            string Res;
            PersianCalendar DateFme = new PersianCalendar();
            Res = DateFme.GetMonth(oDateTime).ToString();
            switch (Res)
            {
                case "1":
                    {
                        Resme = "فروردین";
                        break;
                    }
                case "2":
                    {
                        Resme = "اردیبهشت";
                        break;
                    }
                case "3":
                    {
                        Resme = "خرداد";
                        break;
                    }
                case "4":
                    {
                        Resme = "تیر";
                        break;
                    }
                case "5":
                    {
                        Resme = "مرداد";
                        break;
                    }
                case "6":
                    {
                        Resme = "شهریور";
                        break;
                    }

                case "7":
                    {
                        Resme = "مهر";
                        break;
                    }
                case "8":
                    {
                        Resme = "آبان";
                        break;
                    }
                case "9":
                    {
                        Resme = "آذر";
                        break;
                    }

                case "10":
                    {
                        Resme = "دی";
                        break;
                    }
                case "11":
                    {
                        Resme = "بهمن";
                        break;
                    }
                case "12":
                    {
                        Resme = "اسفند";
                        break;
                    }
            }
            return Resme;
        }

        public string Miladi_To_Shamsi(DateTime miladiDate)
        {
            try
            {
                DateTime MiladiDate = miladiDate;

                int Year = mycal.GetYear(MiladiDate);
                int Month = mycal.GetMonth(MiladiDate);
                int Day = mycal.GetDayOfMonth(MiladiDate);

                string ShamsiDate = Year + "/" + Month + "/" + Day;

                return ShamsiDate;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}