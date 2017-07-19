%%
%function printPrism(links,pins,scale)
function printPrism(numSides, w)
%width is top width in mm

% Print scene to SVG file

% Printing options
% We're using inches.

% OW: change to mm
mmTOin = 0.0393700787;
boardSize = 30*mmTOin; % 12; %initial value is 300*mmTOin
pinDiameter = 4*mmTOin; % 5/16;
pinRadiusOffset = 0.01*mmTOin; % -0.004; % empirically determined
margin = 5*mmTOin; % 1/4;
pinRadius = pinDiameter/2+pinRadiusOffset;
strokeWidth = 3.3*mmTOin; % 1/90; % assuming inches %initial value is 0.3*mmTOin

theta = 180*(numSides - 2)/numSides; %theta is the interior angle of a regular polygon
disp('theta: '+theta);
h = 0.45 * w * tand(theta/2) / sind(45); %height of prism side, tand takes argument in degrees
disp('h: '+h);

filename = uiputfile('*.svg');
if ~filename
    return;
end


filename = strrep(filename, '.svg', '_prism_sides.svg');
file = fopen(filename,'w');
%str = '<svg height="%fin" width="%fin" viewBox="0 0 %f %f">\n';
%fprintf(file,str,boardSize/5,boardSize,boardSize,boardSize/5);
str = '<svg height="%fin" width="%fin">\n';
fprintf(file,str,boardSize/5,boardSize);

color = 'red';

%P is position matrix of vertices for each prism side
P = [0.2*w 1.2*w 0.75*w 0.65*w; 
    0.2*w 0.2*w 0.2*w+h 0.2*w+h; 
    1.00 1.00 1.00 1.00];

for i = 0 : numSides-1
    disp('i: '+i);
       
    P(1,:) = P(1,:) + 1.2*w; % Extract third row
    disp('P: ');
    disp(P);
    %Print prism sides
    %x
    %y
    %coordinates are specified in inches from top left of canvas
    
    svgPolyline(file,P * mmTOin,strokeWidth,color);
end

fprintf(file,'</svg>\n');
fclose(file);

end

%%
function svgPolyline(file,x,strokeWidth,color)
fprintf(file,'<path d="M %f %f ',x(1:2,1));
for i = 2 : size(x,2)
    fprintf(file,'L %f %f ',x(1:2,i));
end
fprintf(file,'Z" style="fill:none;stroke:%s;stroke-width:%f"/>\n',color,strokeWidth);
end

%%
function svgCircle(file,c,r,strokeWidth)
% http://stackoverflow.com/questions/5737975/circle-drawing-with-svgs-arc-path
str = '<path d="M %f, %f m %f, 0 a %f,%f 0 1,0 %f,0 a %f,%f 0 1,0, %f,0 ';
fprintf(file,str,c(1),c(2),-r,r,r,r*2,r,r,-r*2);
fprintf(file,'Z" style="fill:none;stroke:red;stroke-width:%f"/>\n',strokeWidth);
end

%%
%%% SVG MM BEGIN %%%

%%
function svgPolyline_mm(file,x,strokeWidth,color)
fprintf(file,'<path d="M %fmm, %fmm ',x(1:2,1));
for i = 2 : size(x,2)
    fprintf(file,'L %fmm, %fmm ',x(1:2,i));
end
fprintf(file,'Z" style="fill:none;stroke:%s;stroke-width:%fmm"/>\n',color,strokeWidth);
end

%%
function svgCircle_mm(file,c,r,strokeWidth)
% http://stackoverflow.com/questions/5737975/circle-drawing-with-svgs-arc-path
str = '<path d="M %fmm, %fmm m %fmm, 0 a %fmm,%fmm 0 1,0 %fmm,0 a %fmm,%fmm 0 1,0, %fmm,0 ';
fprintf(file,str,c(1),c(2),-r,r,r,r*2,r,r,-r*2);
fprintf(file,'Z" style="fill:none;stroke:red;stroke-width:%fmm"/>\n',strokeWidth);
end

%%% SVG MM END %%%