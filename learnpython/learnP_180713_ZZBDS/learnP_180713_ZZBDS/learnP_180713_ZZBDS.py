#S=r'ABC\-001'
#print(S)

import re
#print(re.match(r'\d{3}-\d{3,8}$','010-12345'))

d='a b  c'
print(d.split(' '))
print(re.split(r'\s+',d))
