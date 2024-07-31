-- Active: 1722319631980@@127.0.0.1@3306

USE `mail_db`;

INSERT INTO `email_templates` (`type`, `header`, `body`)
VALUES
    ('role_email', 'You Role was changed', '<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
        h1 {
            color: #333333;
        }
        p {
            color: #555555;
            line-height: 1.5;
        }
        .button {
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            color: #ffffff;
            background-color: #007bff;
            text-decoration: none;
            border-radius: 5px;
        }
        .footer {
            margin-top: 20px;
            color: #999999;
            font-size: 12px;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>Role Update Notification</h1>
        <p>Dear {{username}},</p>
        <p>We would like to inform you that your role within [Your Company Name] has been updated. Your new role is: <strong>{{role}}</strong>.</p>
        <p>This change may affect your permissions and access levels within our system. Please log in to your account to review your new permissions and ensure you understand the changes.</p>
        <a href="[Account Login URL]" class="button">Log in to Your Account</a>
        <p>If you have any questions or concerns about this change, please contact our support team for assistance.</p>
        <div class="footer">
            <p>&copy; [Year] [Your Company Name]. All rights reserved.</p>
            <p>[Company Address]</p>
        </div>
    </div>
</body>
</html>
'),
('verification_email', 'Verify your email', '<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
        h1 {
            color: #333333;
        }
        p {
            color: #555555;
            line-height: 1.5;
        }
        .button {
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            color: #ffffff;
            background-color: #007bff;
            text-decoration: none;
            border-radius: 5px;
        }
        .footer {
            margin-top: 20px;
            color: #999999;
            font-size: 12px;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>Email Verification</h1>
        <p>Dear {{username}},</p>
        <p>Thank you for signing up with [Your Company Name]! To complete your registration, please verify your email address by clicking the button below:</p>
        <p>You code <span>{{verificationCode}}</span></p>
        <p>If you did not sign up for this account, you can safely ignore this email.</p>
        <div class="footer">
            <p>&copy; [Year] [Your Company Name]. All rights reserved.</p>
            <p>[Company Address]</p>
        </div>
    </div>
</body>
</html>'),
('warning-email', 'Suspicious activity!','<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
        h1 {
            color: #d9534f;
        }
        p {
            color: #555555;
            line-height: 1.5;
        }
        .button {
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            color: #ffffff;
            background-color: #d9534f;
            text-decoration: none;
            border-radius: 5px;
        }
        .footer {
            margin-top: 20px;
            color: #999999;
            font-size: 12px;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>Important Account Notification</h1>
        <p>Dear {{username}},</p>
        <p>We wanted to inform you that there has been a recent change to your account on [Your Company Name]. This change could be:</p>
        <ul>
            <li>Password change</li>
            <li>Profile suspension</li>
        </ul>
        <p>If you did not make this change, it is possible that your account has been compromised. Please take the following steps immediately:</p>
        <ul>
            <li>Reset your password by <a href="[Reset Password URL]" style="color: #007bff; text-decoration: none;">clicking here</a>.</li>
            <li>Review your account activity in your <a href="[Account Activity URL]" style="color: #007bff; text-decoration: none;">account settings</a>.</li>
            <li>Contact our support team if you need further assistance.</li>
        </ul>
        <a href="[Account Settings URL]" class="button">Go to Account Settings</a>
        <p>For your security, please do not share your password or account details with anyone.</p>
        <div class="footer">
            <p>&copy; [Year] [Your Company Name]. All rights reserved.</p>
            <p>[Company Address]</p>
        </div>
    </div>
</body>
</html>'),
('welcome_email', 'Welcome!!!', '<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
        h1 {
            color: #333333;
        }
        p {
            color: #555555;
            line-height: 1.5;
        }
        .button {
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            color: #ffffff;
            background-color: #28a745;
            text-decoration: none;
            border-radius: 5px;
        }
        .footer {
            margin-top: 20px;
            color: #999999;
            font-size: 12px;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>Welcome to [Your Company Name]!</h1>
        <p>Hi {{username}},</p>
        <p>Congratulations! Your email has been successfully verified. Were excited to have you on board at [Your Company Name].</p>
        <p>Here are a few things you can do next:</p>
        <ul>
            <li>Explore our <a href="[Feature Page URL]" style="color: #007bff; text-decoration: none;">features</a> and see how they can help you.</li>
            <li>Visit your <a href="[User Dashboard URL]" style="color: #007bff; text-decoration: none;">dashboard</a> to customize your settings.</li>
            <li>Check out our <a href="[Help Center URL]" style="color: #007bff; text-decoration: none;">Help Center</a> if you have any questions.</li>
        </ul>
        <a href="[User Dashboard URL]" class="button">Go to Your Dashboard</a>
        <p>If you have any questions or need assistance, feel free to reply to this email or contact our support team.</p>
        <div class="footer">
            <p>&copy; [Year] [Your Company Name]. All rights reserved.</p>
            <p>[Company Address]</p>
        </div>
    </div>
</body>
</html>
')
;