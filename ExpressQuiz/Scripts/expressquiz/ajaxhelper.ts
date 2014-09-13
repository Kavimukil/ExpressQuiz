﻿

module ExpressQuiz {
    "use strict";
    export class AjaxHelper {


        static createRequestionVerificationTokenHeader() : any{
            var headers: any;
            var token: string;
            headers = {};
            token = $("#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]").val() ;

            if (token === undefined) {
                token = $("input[name=__RequestVerificationToken]").val();
            }
            headers["__RequestVerificationToken"] = token;
           
            return headers;
        }

    }




}

