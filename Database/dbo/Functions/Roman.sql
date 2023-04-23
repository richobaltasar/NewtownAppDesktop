create function [dbo].[Roman] (
  @arabic int
) returns varchar(30) as begin
  if @arabic > 10000 return '*'
  declare @roman varchar(30)
  set @roman = replicate('M',@arabic/1000)
  set @arabic = @arabic%1000
  set @roman = @roman + replicate('C',@arabic/100)
  set @arabic = @arabic%100
  set @roman = @roman + replicate('X',@arabic/10)
  set @arabic = @arabic%10
  set @roman = @roman + replicate('I',@arabic)

  set @roman = replace(@roman,replicate('C',9),'CM')
  set @roman = replace(@roman,replicate('X',9),'XC')
  set @roman = replace(@roman,replicate('I',9),'IX')

  set @roman = replace(@roman,replicate('C',5),'D')
  set @roman = replace(@roman,replicate('X',5),'L')
  set @roman = replace(@roman,replicate('I',5),'V')

  set @roman = replace(@roman,replicate('C',4),'CD')
  set @roman = replace(@roman,replicate('X',4),'XL')
  set @roman = replace(@roman,replicate('I',4),'IV')

  return @roman
end



